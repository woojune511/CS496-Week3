using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;


public interface ITreeNode<T>{
    T Parent { get; set; }
    bool IsLeaf { get; }
    bool IsRoot { get; }
    T GetRootNode();
}


public abstract class TreeNodeBase<T>: ITreeNode<T> where T: class, ITreeNode<T>{
    protected TreeNodeBase()
    {
        ChildNodes = new List<T>();
    }

    public T Parent
    {
        get;
        set;
    }
    public List<T> ChildNodes
    {
        get;
        private set;
    }

    public bool IsLeaf
    {
        get { return ChildNodes.Count == 0; }
    }

    public bool IsRoot
    {
        get { return Parent == null; }
    }

    public void AddChild(T child)
    {
        child.Parent = MySelf;  // Cant do this as cannot cast this to Type T
        ChildNodes.Add(child);
    }

    public void AddChildren(IEnumerable<T> children)
    {
        foreach (T child in children)
            AddChild(child);
    }

    protected abstract T MySelf
    {
        get;
    }

    public T GetRootNode()
    {
        if (Parent == null)
            return MySelf;

        return Parent.GetRootNode();  // T does not contain a definition for GetRootNode
    }

}

public class RoomTreeNode: TreeNodeBase<RoomTreeNode>{

    public Room room;

    public RoomTreeNode(Room rm){
        room = rm;
    }


    protected override RoomTreeNode MySelf{
        get{
            return this;
        }
    }
}