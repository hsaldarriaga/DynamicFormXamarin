using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using PGC.Module.BusinessObjects.Bos;

using Couchbase.Lite;

using Newtonsoft.Json.Linq;

namespace DynamicForm3.Droid
{
    public class DroidDatabase
    {
        private static DroidDatabase _Instance;

        public static DroidDatabase Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new DroidDatabase();
                return _Instance;
            }
        }
        private DroidDatabase()
        {

        }

        public async Task<bool> CreateDataBase()
        {
            db = Manager.SharedInstance.GetExistingDatabase(DatabaseName);
            bool val1 = true, val2 = true;
            if (db != null)
            {
                val1 = db.GetExistingDocument("Enumeration") == null;
                val2 = db.GetExistingDocument("Forms") == null;
            }
            if (db == null || val1 || val2)
            {
                await Task.Factory.StartNew(() =>
                {
                    if (db == null)
                        db = Manager.SharedInstance.GetDatabase(DatabaseName);
                    String namespace_BO = "PGC.Module.BusinessObjects.Bos";
                    Type[] tipos = typeof(BO00000).Assembly.GetTypes().Where((t) => String.Equals(t.Namespace, namespace_BO, StringComparison.Ordinal)).ToArray();
                    Type[] classes, enumtypes;
                    if (val1)
                    {
                        enumtypes = tipos.Where((t) => t.IsEnum && t.Name.StartsWith("FVL")).ToArray();
                        if (!SaveEnumeration(db, enumtypes))
                            return false;
                    }
                    if (val2)
                    {
                        classes = tipos.Where((t) => t.Name.StartsWith("BO")).ToArray();
                        if (!SaveBusinessObject(db, classes))
                            return false; 
                    }
                    return true;
                });
            }
            return true;
        }
        private bool SaveEnumeration(Database db, Type[] enum_tps)
        {
            Document dc = db.GetDocument("Enumeration");
            if (dc == null)
                return false;
            List<object> enum_values = new List<object>();
            foreach (var item in enum_tps)
            {
                var dicc = new Dictionary<string, object>();
                dicc.Add("enum_id", item.Name);
                
                List<object> values = new List<object>();
                foreach (var m in item.GetEnumValues())
                {
                    FieldInfo info = item.GetField(m.ToString());
                    var val = info.GetCustomAttribute<DevExpress.ExpressApp.DC.XafDisplayNameAttribute>();
                    String str = ((int)m) + ";" + val.Caption;
                    values.Add(str);
                }
                dicc.Add("values", values);
                enum_values.Add(dicc);
            }
            dc.PutProperties(new Dictionary<string, object>() {
                {"enumerations", enum_values}
            });
            return true;
        }
        private bool SaveBusinessObject(Database db, Type [] bo_tps)
        {
            Document dc = db.GetDocument("Forms");
            if (dc == null)
                return false;
            List<object> valores = new List<object>();
            foreach (var item in bo_tps)
            {
                var bo = new Dictionary<string, object>();
                bo.Add("bo_id", item.Name);
                bo.Add("description", item.GetCustomAttribute<System.ComponentModel.DescriptionAttribute>().Description);
                var prop_tps = item.GetProperties();
                var properties = new List<Dictionary<string, object>>();
                int k = 0;
                foreach (var p in prop_tps)
                {
                    var prop = new Dictionary<string, object>();
                    var cp = p.GetCustomAttribute<ExtremeGenerator.Metadata.BusinessObject.ExtremeFieldCaption>();
                    if (cp != null) // Es un campo
                    {
                        string caption = cp.Caption;
                        int id = p.GetCustomAttribute<ExtremeGenerator.Metadata.BusinessObject.UIIndex>().EditViewIndex;
                        prop.Add("prop_id", p.Name);
                        prop.Add("prop_view_order", id);
                        prop.Add("caption", caption);
                        if (cp.IsOnlyLabel)
                            prop.Add("field_type", 4);
                        else
                        {
                            var criteria = p.GetCustomAttribute<ExtremeGenerator.Metadata.Editors.ExtremeKoIF>();
                            if (criteria != null)
                                prop.Add("criteria", criteria.Criteria);
                            Type prop_tp = p.PropertyType;
                            if (p.PropertyType == typeof(string))
                                prop.Add("field_type", 1);
                            else if (p.PropertyType == typeof(int))
                            {
                                var isEnumeration = p.GetCustomAttribute<ExtremeGenerator.Metadata.Editors.ExtremeArrayDataSource>();
                                if (isEnumeration != null)
                                {
                                    prop.Add("field_type", 2);
                                    prop.Add("datasource", isEnumeration.DataSourceId);
                                    prop.Add("title", isEnumeration.Title);
                                }
                                else
                                {
                                    prop.Add("field_type", 5);
                                    if (k + 1 < prop_tps.Length)
                                    {
                                        var pp = prop_tps[k + 1];
                                        var att = pp.GetCustomAttribute<DevExpress.Persistent.Base.PersistentAliasAttribute>();
                                        if (att != null)
                                            prop.Add("calculo", att.PropertyName);
                                    }
                                }
                            }
                            else if (p.PropertyType == typeof(DateTime))
                            {
                                var date_format = p.GetCustomAttribute<ExtremeGenerator.Metadata.Editors.ExtremeDateFormat>();
                                if (date_format != null)
                                    prop.Add("field_type", 0);
                                else
                                    prop.Add("field_type", 8);
                            }
                            else if (p.PropertyType == typeof(bool))
                                prop.Add("field_type", 9);
                            else if (p.PropertyType == typeof(decimal))
                                prop.Add("field_type", 7);
                            else if (p.PropertyType == typeof(Double))
                            {
                                prop.Add("field_type", 13);
                                if (k + 1 < prop_tps.Length)
                                {
                                    var pp = prop_tps[k + 1];
                                    var att = pp.GetCustomAttribute<DevExpress.Persistent.Base.PersistentAliasAttribute>();
                                    if (att != null)
                                        prop.Add("calculo", att.PropertyName);
                                }
                            }
                            else if (p.PropertyType.Name.StartsWith("BO"))
                            {
                                var cus = p.GetCustomAttribute<ExtremeGenerator.Metadata.BusinessObject.ExtremeLinkedObject>();
                                if (cus != null)
                                    prop.Add("field_type", 11);
                                else
                                    prop.Add("field_type", 3);
                                prop.Add("link_to", p.PropertyType.Name);
                            }
                            else if (p.PropertyType.Name.StartsWith("XPCollection"))
                            {
                                prop.Add("field_type", 12);
                                prop.Add("link_to", prop_tp.GetGenericArguments()[0].Name);
                            }
                            else
                            {
                                Console.WriteLine(p.ReflectedType.Name + p.Name);
                            }
                        }
                        properties.Add(prop);
                    }
                    k++;
                }
                bo.Add("fields", properties);
                valores.Add(bo);

            }
            dc.PutProperties(new Dictionary<string, object>()
            {
                {"BOs", valores}
            });
            return true;
        }

        public async Task<Dictionary<string,string>> getAllForms()
        {
            Document doc = db.GetDocument("Forms");
            if (doc != null)
            {
                return await Task<Dictionary<string, string>>.Factory.StartNew(() =>
                    {
                        var values = new Dictionary<string, string>();
                        var list = doc.Properties["BOs"] as JArray;
                        foreach (var item in list)
                        {
                            values.Add(item["bo_id"].ToString(), item["description"].ToString());
                        }
                        return values;
                    });
            }
            return null;
        }

        public JToken getForm(string id)
        {
            Document doc = db.GetDocument("Forms");
            if (doc != null)
            {
                var list = doc.Properties["BOs"] as JArray;
                foreach (var item in list)
                {
                    if (item["bo_id"].ToString() == id) {
                        return item;
                    }
                }
                return null;
            }
            return null;
        }

        public Dictionary<string, int> getEnumeration(string id)
        {
            Document dc = db.GetDocument("Enumeration");
            if (dc != null)
            {
                var values = new Dictionary<string, int>();
                var root = dc.Properties["enumerations"] as JArray;
                var elem = root.First((e) => e["enum_id"].ToString() == id);
                var enum_values = elem["values"];
                foreach (var item in enum_values)
                {
                    string[] val = item.ToString().Split(';');
                    values.Add(val[1], Int32.Parse(val[0]));
                }
                return values;
            }

            return null;
        }

        public Dictionary<string, object> getDataBO(string id)
        {
            var doc = db.GetDocument(id);
            return new Dictionary<string, object>
                {
                    { "DocumentID", id},
                    { "Document_Values", doc.Properties}
                };
        }

        public List<Dictionary<string, object>> getDataBOList(string id, bool IncludeLink)
        {
            var query = db.CreateAllDocumentsQuery();
            var resu = query.Run();
            var resultados = new List<Dictionary<string,object>>();
            foreach (var item in resu)
            {
                if (item.Document.Properties == null)
                    continue;
                if (!item.Document.Properties.ContainsKey("isLink"))
                    continue;
                object val = item.Document.Properties["isLink"];
                object idd = item.Document.Properties["bo_id"];
                if (val != null && idd != null)
                {
                    bool tf = bool.Parse(val.ToString());
                    if (tf == IncludeLink && idd.ToString().Equals(id))
                        resultados.Add(new Dictionary<string, object>
                        {
                            { "DocumentID", item.DocumentId},
                            { "Document_Values", item.Document.Properties.ToDictionary( e => e.Key, e => e.Value)}
                        });
                }
            }
            return resultados;
        }

        public string CreateBOData(List<Dictionary<string, object>> data, string id, bool isLink)
        {
            Document doc = db.CreateDocument();
            if (doc == null)
                return null;
            Dictionary<string, object> values = new Dictionary<string, object>();
            values.Add("bo_id", id);
            values.Add("date", DateTime.Now.ToString("MM/dd/yy"));
            values.Add("field_values", data);
            values.Add("isLink", isLink.ToString());
            doc.PutProperties(values);
            return doc.Id;
        }

        public Dictionary<string, object> ReadBOData(string id)
        {
            Document doc = db.GetDocument(id);
            return doc.Properties.ToDictionary( e => e.Key, e => e.Value);
        }

        public bool UpdateBOData(Dictionary<string, object> data, string id, bool isLink)
        {
            Document doc = db.GetExistingDocument(id);
            if (doc == null)
                return false;
            data.Add("date", DateTime.Now.ToString("MM/dd/yy"));
            data.Add("isLink", isLink.ToString());
            data.Add("_rev", doc.Properties["_rev"]);
            doc.PutProperties(data);
            return true;
        }

        public bool DeleteBOData(string id)
        {
            Document doc = db.GetExistingDocument(id);
            if (doc == null)
                return false;
            doc.Delete();
            return true;
        }

        public static readonly string DatabaseName = "dynamicformdatabase";
        private Database db;
    }
}