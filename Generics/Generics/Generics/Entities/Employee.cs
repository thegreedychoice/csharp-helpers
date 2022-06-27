using System;
namespace Generics.Entities
{
    public class Employee: EntityBase
	{
		public string? FirstName { get; set; }

		public override string ToString() => $"Id: {Id}, FirstName: {FirstName}";
	}
}

