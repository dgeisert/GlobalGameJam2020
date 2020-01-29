using System;
using System.Linq;
using System.Collections.Generic;

public interface DependencyNode<T>
{
	void DependsOn(params T[] dependencies);
}

public class DependencyGraph<T>
{
	private Dictionary<T, List<T>> dependents = new Dictionary<T, List<T>>();
	private Dictionary<T, List<T>> dependencies = new Dictionary<T, List<T>>();

	public int Size
	{
		get
		{
			return dependents.Count;
		}
	}

	public List<T> Roots
	{
		get
		{
			return dependencies
				.Where(kvp => kvp.Value.Count == 0)
				.Select(kvp => kvp.Key)
				.ToList();
		}
	}

	public DependencyNode<T> AddItem(T item)
	{
		if (dependents.ContainsKey(item))
		{
			throw new ArgumentException("Item already exists.");
		}

		dependents[item] = new List<T>();
		dependencies[item] = new List<T>();

		return new Node<T>(this, item);
	}

	public List<T> GetDependents(T item)
	{
		MustExist(item, dependents);
		return dependents[item];
	}

	public List<T> GetDependencies(T item)
	{
		MustExist(item, dependencies);
		return dependencies[item];
	}

	private void MustExist(T item)
	{
		MustExist(item, dependents);
	}

	private void MustExist(T item, Dictionary<T, List<T>> map)
	{
		if (!map.ContainsKey(item))
		{
			throw new ArgumentException("item not found");
		}
	}

	private class Node<V>: DependencyNode<V>
	{
		private DependencyGraph<V> graph;
		private V item;

		public Node(DependencyGraph<V> graph, V item)
		{
			this.graph = graph;
			this.item = item;
		}

		public void DependsOn(params V[] dependencies)
		{
			foreach (var dependency in dependencies)
			{
				graph.MustExist(dependency);
				AddItemToList(item, dependency, graph.dependencies);
				AddItemToList(dependency, item, graph.dependents);
			}
		}

		private void AddItemToList(V key, V toAdd, Dictionary<V, List<V>> map)
		{
			var list = map[key];
			if (!list.Contains(toAdd))
			{
				list.Add(toAdd);
			}
		}
	}
}