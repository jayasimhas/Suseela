using System;
using System.Collections.Generic;
using System.Linq;
using Informa.Library.Utilities.DataModels;
using Informa.Models.DCD;
using Jabberwocky.Autofac.Attributes;

namespace Informa.Library.DCD
{
    public interface IDcdContentAnalyzer
    {
        string GetParentCompany(CompanyContent content);
        TreeNode<string,string>[] GetCompanySubsidiaryTree(ParentsAndDivisions content);
        TreeNode<string,string>[] GetCodingSetTrees(Coding[] codings, string delimeter);
    }

    [AutowireService]
    public class DcdContentAnalyzer : IDcdContentAnalyzer
    {


        public TreeNode<string,string>[] GetCodingSetTrees(Coding[] codings, string delimeter)
        {
            if (!codings.Any()) return null;

            IEnumerable<DcdPath> paths =
                codings.Select(
                    coding => new DcdPath
                    {
                        Segments = coding.Name.Split(new[] {delimeter}, StringSplitOptions.RemoveEmptyEntries)
                    });

            return CreateSegmentsTrees(paths);

        }

        public string GetParentCompany(CompanyContent content)
        {
            return content.ParentsAndDivisions.CompanyPaths.FirstOrDefault()?.Path;
        }

        public TreeNode<string,string>[] GetCompanySubsidiaryTree(ParentsAndDivisions content)
        {
            IEnumerable<DcdPath> paths =
                content.CompanyPaths.Select(
                    company => new DcdPath
                    {
                        Id = company.Id,
                        Segments = company.Path.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries)
                    });

            return CreateSegmentsTrees(paths);
        }

        private TreeNode<string, string>[] CreateSegmentsTrees(IEnumerable<DcdPath> paths)
        {
            var nodeSet = new Dictionary<string, TreeNode<string, string>>();
            foreach (var path in paths)
            {
                for (var i = 0; i < path.Segments.Length; i++)
                {
                    CreateNode(path, i, ref nodeSet);
                }
            }
            return nodeSet.Values.Where(x => x.Parent == null).ToArray();
        }

        private void CreateNode(DcdPath path, int segment, ref Dictionary<string, TreeNode<string, string>> nodeSet)
        {
            var key = path.Segments[segment];
            if (nodeSet.ContainsKey(key)) return;

            var newNode = new TreeNode<string, string>(key) {Data = path.Id};

            if (segment > 0)
            {
                var parentKey = path.Segments[segment - 1];
                if (!nodeSet.ContainsKey(parentKey))
                {
                    CreateNode(path, segment - 1, ref nodeSet);
                }
                newNode.Parent = nodeSet[parentKey];                
                newNode.Parent.Children.Add(newNode);
            }
            nodeSet.Add(key, newNode);
        }

        public class DcdPath
        {
            public string Id;
            public string[] Segments;
        }
    }
}