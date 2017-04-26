using GraphQL;
using UnityEngine;

namespace GraphQL {
	public interface IQueryValidator {
		QueryValidator.ValidationError[] Validate(Query query);
	}
}
