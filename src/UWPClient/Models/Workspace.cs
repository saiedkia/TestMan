using System.Collections.Generic;

namespace UWPClient.Models
{
    public class Workspace
    {
        public List<WorkspaceItem> Workspaces { get; set; }
        public string Token { get; set; }

        public Workspace()
        {
            Workspaces = new List<WorkspaceItem>();
        }

        public Workspace(List<WorkspaceItem> items)
        {
            Workspaces = items;
        }
    }

    public struct WorkspaceItem
    {
        public string Name { get; set; }
        public string Path { get; set; }

        public WorkspaceItem(string name, string path) : this()
        {
            Name = name;
            Path = path;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
