using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace GraphQL {
	namespace Validators {
		public class FileNameValidator : IQueryValidator {
			private QueryValidator.ValidationError Error {
				get {
					QueryValidator.ValidationError error = new QueryValidator.ValidationError();
					error.Message = "File name is invalid, only include alphanumeric characters and make sure it's unique";
					error.LineNumber = 0;
					return error;
				}
			}

			public QueryValidator.ValidationError[] Validate(Query query) {
				Regex pattern = new Regex("[^a-zA-Z0-9-]", RegexOptions.Compiled);
				string fixedName = pattern.Replace(query.name, "");

				List<QueryValidator.ValidationError> errors = new List<QueryValidator.ValidationError>();

				if (fixedName == "") {
					errors.Add(Error);
				} else {
					Debug.Log(fixedName);
					query.name = fixedName;
				}

				return errors.ToArray();
			}
		}
	}
}
