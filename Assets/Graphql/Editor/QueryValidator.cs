using System.Collections.Generic;
using GraphQL.Validators;

namespace GraphQL {
	public class QueryValidator {
		public struct ValidationError {
			public string Message;
			public int LineNumber;
		}

		public IQueryValidator[] Validators = new IQueryValidator[] {
			new FileNameValidator()
		};

		public Query Query;

		public QueryValidator(Query query) {
			Query = query;
		}

		public ValidationError[] Validate() {
			List<ValidationError> errors = new List<ValidationError>();

			foreach (IQueryValidator validator in Validators) {
				errors.AddRange(validator.Validate(Query));
			}

			return errors.ToArray();
		}
	}
}
