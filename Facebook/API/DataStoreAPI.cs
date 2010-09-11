using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;
using Facebook.Entity;
using Facebook.Exceptions;
using Facebook.Parser;

namespace Facebook.API
{
	/// <summary>
	/// DataStore methods and objects contributed by Rabbit!
	/// </summary>
	public partial class FacebookAPI
	{
		/// <summary>
		/// Creates a new object type.
		/// </summary>
		/// <param name="typeName">Name of this new object type. 
		/// This name needs to be unique among all object types and associations 
		/// defined for this application. This name also needs to be a valid 
		/// identifier, which is no longer than 32 characters, starting with
		/// a letter (a-z) and consisting of only small letters (a-z), 
		/// numbers (0-9) and/or underscores.</param>
		public void CreateObjectType(string typeName)
		{
			if (String.IsNullOrEmpty(typeName))
			{
				throw new ArgumentNullException("typeName");
			}
			CheckDataName(ref typeName);

			var query = new Dictionary<string, string>(2) { { "method", "facebook.data.createObjectType" }, { "name", typeName } };

			ExecuteApiCall(query, true);
		}

		/// <summary>
		/// Remove a previously defined object type.
		/// This will also delete ALL objects of this type.
		/// </summary>
		/// <param name="typeName">Name of the type to delete.</param>
		/// <remarks>This deletion is NOT reversible.</remarks>
		public void DropObjectType(string typeName)
		{
			if (String.IsNullOrEmpty(typeName))
			{
				throw new ArgumentNullException("typeName");
			}

			if (typeName.Length > 32 || !Char.IsLetter(typeName, 0))
			{
				throw new FacebookInvalidObjectTypeNameException();
			}

			// Convert the type name to lower case
			typeName = typeName.ToLowerInvariant();
			var query = new Dictionary<string, string>(2) { { "method", "facebook.data.dropObjectType" }, { "obj_type", typeName } };

			ExecuteApiCall(query, true);
		}

		/// <summary>
		/// Add a new object property to an object type.
		/// </summary>
		/// <param name="typeName"></param>
		/// <param name="propertyName">Name of the new property to add. 
		/// This name needs to be a valid identifier, which is no longer 
		/// than 32 characters, starting with a letter (a-z) and consisting
		/// of only small letters (a-z), numbers (0-9) and/or underscores.</param>
		/// <param name="propertyType">Type of the new property.</param>
		public void DefineObjectProperty(string typeName, string propertyName, ObjectPropertyType propertyType)
		{
			if (String.IsNullOrEmpty(typeName))
			{
				throw new ArgumentNullException("typeName");
			}

			if (String.IsNullOrEmpty(propertyName))
			{
				throw new ArgumentNullException("propertyName");
			}

			CheckDataName(ref typeName);
			CheckDataName(ref propertyName);

			var query = new Dictionary<string, string>(4)
                   	{
                   		{"method", "facebook.data.defineObjectProperty"},
                   		{"obj_type", typeName},
                   		{"prop_name", propertyName},
                   		{"prop_type", ((int) propertyType).ToString()}
                   	};

			ExecuteApiCall(query, true);
		}

		/// <summary>
		/// Get a list of all previously defined object types.
		/// </summary>
		public Collection<ObjectTypeInfo> GetObjectTypes()
		{
			var query = new Dictionary<string, string>(1) { { "method", "facebook.data.getObjectTypes" } };
			var result = new Collection<ObjectTypeInfo>();

			XmlNodeList nodeList;
			GetMultipleNodes(ExecuteApiCallString(query, true), "data_getObjectTypes_response", "object_type_info", out nodeList);

			foreach (XmlNode node in nodeList)
			{
				result.Add(ObjectTypeInfoParser.ParseObjectTypeInfo(node));
			}

			return result;
		}

		public ObjectTypeInfo GetObjectType(string typeName)
		{
			if (String.IsNullOrEmpty(typeName))
			{
				throw new ArgumentNullException("typeName");
			}
			CheckDataName(ref typeName);

			var parameterList = new Dictionary<string, string>(2) { { "method", "facebook.data.getObjectType" }, { "obj_type", typeName } };

			XmlNode node;
			if (!GetSingleNode(ExecuteApiCallString(parameterList, true), "data_getObjectType_response", out node)) return null;

			var result = ObjectTypeInfoParser.ParseObjectTypeInfo(node);
			result.Name = typeName;
			return result;
		}

		/// <summary>
		/// Creates a new object of the given type.
		/// </summary>
		/// <param name="typeName"></param>
		/// <param name="properties"></param>
		/// <returns></returns>
		public long CreateObject(string typeName, Dictionary<string, string> properties)
		{
			if (String.IsNullOrEmpty(typeName))
			{
				throw new ArgumentNullException("typeName");
			}

			if (typeName.Length > 32 || !Char.IsLetter(typeName, 0))
			{
				throw new FacebookInvalidObjectTypeNameException();
			}

			// Convert the type name to lower case
			typeName = typeName.ToLowerInvariant();
			var parameterList = new Dictionary<string, string>(3)
			                    	{
			                    		{"method", "facebook.data.createObject"},
			                    		{"obj_type", typeName},
			                    		{"properties", ToJsonAssociativeArray(properties)}
			                    	};

			XmlNode node;
			return GetSingleNode(ExecuteApiCallString(parameterList, true), "data_createObject_response", out node) ? Int64.Parse(node.InnerText) : -1;
		}

		/// <summary>
		/// Update an object's properties.
		/// </summary>
		/// <param name="objectID">Object ID.</param>
		/// <param name="properties">Name-value pairs of new properties.</param>
		/// <param name="replaceExisitingProperties">True if replace all existing properties; false to merge into existing ones.</param>
		public void UpdateObject(long objectID, Dictionary<string, string> properties, bool replaceExisitingProperties)
		{
			if (properties == null)
			{
				properties = new Dictionary<string, string>();
			}

			var parameterList = new Dictionary<string, string>(4)
			                    	{
			                    		{"method", "facebook.data.updateObject"},
			                    		{"obj_id", objectID.ToString()},
			                    		{"properties", ToJsonAssociativeArray(properties)},
			                    		{"replace", replaceExisitingProperties.ToString().ToLowerInvariant()}
			                    	};

			ExecuteApiCall(parameterList, true);
		}

		/// <summary>
		/// Delete an object permanently.
		/// </summary>
		/// <param name="objectID"></param>
		public void DeleteObject(long objectID)
		{
			var parameterList = new Dictionary<string, string>(2)
			                    	{
			                    		{"method", "facebook.data.deleteObject"},
			                    		{"obj_id", objectID.ToString()}
			                    	};

			ExecuteApiCall(parameterList, true);
		}

		/// <summary>
		/// Get object with all properties.
		/// </summary>
		/// <param name="objectID">Object ID.</param>
		/// <param name="typeName">The name of object type.</param>
		/// <returns></returns>
		/// <remarks>If the object not exists, all property values will
		/// be String.Empty.</remarks>
		public DataObject GetObject(long objectID, string typeName)
		{
			if (String.IsNullOrEmpty(typeName))
			{
				throw new ArgumentNullException("typeName");
			}
			CheckDataName(ref typeName);

			// Get all columns of the object type
			var typeInfo = GetObjectType(typeName);
			var properties = new List<string>();
			foreach (var pInfo in typeInfo.Properties)
			{
				properties.Add(pInfo.Name);
			}

			return GetObject(objectID, typeName, properties);
		}

		/// <summary>
		/// Get object with specified properties.
		/// </summary>
		/// <param name="objectID">Object ID.</param>
		/// <param name="typeName">The name of object type.</param>
		/// <param name="properties">Properties to get.</param>
		/// <returns></returns>
		public DataObject GetObject(long objectID, string typeName, List<string> properties)
		{
			if (String.IsNullOrEmpty(typeName))
			{
				throw new ArgumentNullException("typeName");
			}

			if (properties == null || properties.Count == 0)
			{
				throw new ArgumentNullException("properties");
			}

			CheckDataName(ref typeName);

			var columns = String.Join(",", properties.ToArray());
			var query = String.Format("SELECT {0} FROM app.{1} WHERE _id={2}", columns, typeName, objectID);

			XmlNode node;
			if (!GetSingleNode(DirectFQLQuery(query), "fql_query_response", out node)) return null;

			var result = DataObjectParser.ParseDataObject(node);
			result.ID = objectID;
			return result;
		}

		/// <summary>
		/// Defines an object association.
		/// </summary>
		/// <param name="name">Name of forward association to create.</param>
		/// <param name="type">Type of this association.</param>
		/// <param name="info1">Describes object identifier 1 in an association.</param>
		/// <param name="info2">Describes object identifier 2 in an association.</param>
		/// <param name="inverse">(Optional) Name of backward association, if this is a two-way asymmetric one.</param>
		public void DefineAssociation(string name, DataAssociationType type,
			DataAssociationInfo info1, DataAssociationInfo info2, string inverse)
		{
			if (String.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name");
			}

			string info1Alias = info1.Alias;
			string info2Alias = info2.Alias;

			CheckDataName(ref name);
			CheckDataName(ref info1Alias);
			CheckDataName(ref info2Alias);

			var info1Prop = new Dictionary<string, string> {{"alias", info1Alias}};
			AddParameter(info1Prop, "object_type", info1.ObjectType);
			if (info1.Unique) info1Prop.Add("unique", info1.Unique.ToString().ToLowerInvariant());

			var info2Prop = new Dictionary<string, string> {{"alias", info2Alias}};
			AddParameter(info2Prop, "object_type", info2.ObjectType);
			if (info2.Unique) info2Prop.Add("unique", info2.Unique.ToString().ToLowerInvariant());

			var query = new Dictionary<string, string>(6)
			            	{
			            		{"method", "facebook.data.defineAssociation"},
			            		{"name", name},
			            		{"assoc_type", ((int) type).ToString()},
			            		{"assoc_info1", ToJsonAssociativeArray(info1Prop)},
			            		{"assoc_info2", ToJsonAssociativeArray(info2Prop)}
			            	};
			AddParameter(query, "inverse ", inverse);

			ExecuteApiCall(query, true);
		}

		public void UndefineAssociation(string name)
		{
			if (String.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name");
			}
			CheckDataName(ref name);

			var query = new Dictionary<string, string>(2) { { "method", "facebook.data.undefineAssociation" }, { "name", name } };

			ExecuteApiCall(query, true);
		}

		/// <summary>
		/// Creates an association between two object identifiers.
		/// </summary>
		/// <param name="name">Name of the association to set.</param>
		/// <param name="id1">Object identifier 1.</param>
		/// <param name="id2">Object identifier 2.</param>
		/// <param name="data">(Optional) An arbitrary data (max. 255 characters) to store with this association.</param>
		public void SetAssociation(string name, long id1, long id2, string data)
		{
			if (String.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name");
			}
			CheckDataName(ref name);

			var query = new Dictionary<string, string>(2)
			            	{
			            		{"method", "facebook.data.setAssociation"},
			            		{"name", name},
			            		{"obj_id1", id1.ToString()},
			            		{"obj_id2", id2.ToString()}
			            	};
			if (!String.IsNullOrEmpty(data))
			{
				query.Add("data", data);
			}

			ExecuteApiCall(query, true);
		}

		/// <summary>
		/// Returns a list of object ids that are associated with specified object.
		/// </summary>
		/// <param name="name">Name of the association.</param>
		/// <param name="objID">Object identifier.</param>
		/// <param name="noData">True if only return object identifiers; false to return data and time as well.</param>
		/// <returns></returns>
		public List<DataAssociation> GetAssociatedObjects(string name, long objID, bool noData)
		{
			if (String.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name");
			}
			CheckDataName(ref name);

			var query = new Dictionary<string, string>(4)
			            	{
			            		{"method", "facebook.data.getAssociatedObjects"},
			            		{"name", name},
			            		{"obj_id", objID.ToString()},
			            		{"no_data", noData.ToString().ToLowerInvariant()}
			            	};

			var result = new List<DataAssociation>();
			XmlNodeList nodeList;
			GetMultipleNodes(ExecuteApiCallString(query, true), "data_getAssociatedObjects_response", "object_association", out nodeList);

			foreach (XmlNode node in nodeList)
			{
				var asso = DataAssociationParser.ParseDataAssociation(node);
					asso.ID1 = objID;
					result.Add(asso);
			}

			return result;
		}

		/// <summary>
		/// Executes API call to Facebook functions.
		/// </summary>
		/// <param name="method">Method name.</param>
		/// <param name="useSession"></param>
		/// <param name="para"></param>
		/// <returns></returns>
		public XmlDocument ExecuteApiCall(string method, bool useSession, params KeyValuePair<string, string>[] para)
		{
			if (String.IsNullOrEmpty(method))
			{
				throw new ArgumentNullException("method");
			}

			var paramaters = new Dictionary<string, string> { { "method", method } };

			if (null != para)
			{
				foreach (var p in para)
				{
					paramaters.Add(p.Key, p.Value);
				}
			}

			return ExecuteApiCall(method, useSession);
		}

		private string ToListString(IEnumerable<string> properties)
		{
			var list = new List<string>(properties);
			return string.Join(",", list.ToArray());
		}

		private static void CheckDataName(ref string typeName)
		{
			if (typeName.Length > 32 || !Char.IsLetter(typeName, 0))
			{
				throw new FacebookInvalidObjectTypeNameException();
			}

			typeName = typeName.ToLowerInvariant();
		}
	}
}