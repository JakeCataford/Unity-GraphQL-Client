using GraphQL;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GraphQL.Query))]
public class QueryEditor : Editor {
	private Query Query {
		get { return (Query) target; }
	}

	public override void OnInspectorGUI() {
		string newText = EditorGUILayout.TextArea(Query.QueryText);

		if (Query.QueryText != newText) {
			OnQueryChanged();
			Query.QueryText = newText;
		}

		if (GUILayout.Button("Generate Graphql Types")) {
			GenerateTypes();
		}
	}

	private void OnQueryChanged() {
		QueryValidator validator = new QueryValidator(Query);
		QueryValidator.ValidationError[] errors = validator.Validate();
	}

	private void GenerateTypes() {
		TypeGenerator.GenerateTypes();
	}
}
