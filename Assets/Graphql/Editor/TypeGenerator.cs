using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace GraphQL {
    public class TypeGenerator {
        public static void GenerateTypes() {
	        var queries = allQueries;

	        if (File.Exists(FilePath)) {
		        File.Delete(FilePath);
	        }

		    StringBuilder builder = new StringBuilder("// Generated file! do not edit!");
	        builder.AppendLine("");
	        builder.AppendLine("using GraphQL;");

	        Debug.Log(queries.Length);
	        foreach (var query in queries) {
		        Debug.Log(query);
		        GenerateQueryFile(ref builder, query);
	        }

	        StreamWriter writer = File.CreateText(FilePath);
	        writer.Write(builder.ToString());
	        writer.Close();

	        AssetDatabase.Refresh();
        }

	    private static Query[] allQueries {
		    get {
			    string[] gids = AssetDatabase.FindAssets("t:GraphQL.Query");

			    List<Query> queries = new List<Query>();

			    foreach (var gid in gids) {
				    string path = AssetDatabase.GUIDToAssetPath(gid);
				    queries.Add(AssetDatabase.LoadAssetAtPath<GraphQL.Query>(path));
			    }

			    return queries.ToArray();
		    }
	    }

	    private static string FilePath {
		    get { return "Assets/GraphQLGeneratedTypes.cs"; }
	    }

	    private static void GenerateQueryFile(ref StringBuilder builder, Query query) {
		    Debug.Log(builder.ToString());
		    builder.AppendLine("public class " + query.name + "Query : GeneratedQuery {");
		    builder.AppendLine("\toverride public string QueryContent() {");
		    builder.AppendLine("\t\treturn @\"" + query.QueryText + "\";");
		    builder.AppendLine("\t}");
		    builder.AppendLine("}");
	    }
    }
}
