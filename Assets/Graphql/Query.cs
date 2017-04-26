using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphQL {
	[CreateAssetMenu(fileName = "New Graphql Query", menuName = "GraphQL/Query")]
	public class Query : ScriptableObject {
		public string QueryText;
	}
}