using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Informa.Library.Utilities.DataModels;
using Informa.Library.Utilities.Extensions;
using Informa.Models.DCD;
using Jabberwocky.Autofac.Attributes;
using Jabberwocky.Core.Caching;

namespace Informa.Library.DCD
{
    public interface ICompanyContentAnalyzer
    {
        string GetParentCompany(CompanyContent content);
        TreeNode<string> GetSubsidiaryTree(ParentsAndDivisions content);
    }

    [AutowireService]
    public class CompanyContentAnalyzer : ICompanyContentAnalyzer
    {
        public string GetParentCompany(CompanyContent content)
        {
            return content.ParentsAndDivisions.CompanyPaths.FirstOrDefault()?.Path;
        }

        public TreeNode<string> GetSubsidiaryTree(ParentsAndDivisions content)
        {
            List<CompanyPath> paths =
                content.CompanyPaths.Select(
                    path =>
                        path.Alter(i => i.Segments = i.Path.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries)))
                    .ToList();

            return MakeTree(paths, 0);
        }

        private TreeNode<string> MakeTree(List<CompanyPath> paths, int curSegment)
        {
            var node = new TreeNode<string>(paths[0].Segments[curSegment], paths[0].Id.ToString());

            GroupUnderParent(paths.Skip(1).ToList(), (curSegment + 1), ref node);

            return node;
        }

        private void GroupUnderParent(List<CompanyPath> paths, int curSegment, ref TreeNode<string> parent)
        {
            if(paths == null || paths.Count < 1) { return; }

            var group = new List<CompanyPath>();
            string currentRootValue = null;
            foreach (var path in paths)
            {
                if (path.Segments[curSegment] == currentRootValue)
                {
                    group.Add(path);
                }
                else
                {
                    if (group.Any())
                    {
                        var child = MakeTree(group, curSegment);
                        if (child != null) parent.Children.Add(child);
                    }

                    currentRootValue = path.Segments[curSegment];
                    group = new List<CompanyPath> {path};
                }
            }
            if (group.Any())
            {
                var child = MakeTree(group, curSegment);
                if (child != null) parent.Children.Add(child);
            }
        }
    }
}