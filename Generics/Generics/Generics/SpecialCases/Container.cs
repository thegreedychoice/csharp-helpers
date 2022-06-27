using System;
namespace Generics.SpecialCases
{
    public class ContainerBase
    {
        public ContainerBase() => InstanceCountBase++;

        public static int InstanceCountBase { get; private set; }
    }

    public class Container<T>: ContainerBase
    {
		public Container() => InstanceCount++;

        public static int InstanceCount { get; private set; }

        public void PrintItem<TItem>(TItem item)
        {
            Console.WriteLine($"Item: {item}");
        }
	}
}

