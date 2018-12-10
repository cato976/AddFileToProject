using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace AddFileToProject
{
    public class ProjectManager
    {
        public string GetProjectPath(string pathToStartWith)
        {
            var projectFile = string.Empty;
            var currentPath = pathToStartWith;

            while(projectFile == string.Empty)
            {
                var filesThatMatch = GetFilesOfExtension(currentPath, "csproj");
                if(filesThatMatch.Count > 0)
                {
                    return filesThatMatch[0];
                }
                else
                {
                    currentPath = Directory.GetParent(currentPath).FullName;
                }
            }

             return string.Empty;
        }

        public XElement GetPathToCompiledFiles(string pathToProjectFile)
        {
            XElement project = GetProjectNode(pathToProjectFile);

            var result = project.Elements().Where(itemGroup => itemGroup.Name.LocalName == "ItemGroup");
            var result2 = result.First(x => x.Elements().Where(f => f.Name.LocalName == "Compile").Count() > 0);

            return result2;
        }

        public XElement AddFileToProject(string directoryPath, string pathToNewFile)
        {
            var foundAProjectFile = GetFilesOfExtension(Path.GetDirectoryName(directoryPath), "csproj");

            if (foundAProjectFile.Count < 1)
            {
                // go up a level
                return AddFileToProject(Path.GetDirectoryName(directoryPath), pathToNewFile);
            }
            var orginal = GetProjectNode(foundAProjectFile.First());

            XNamespace name = "http://schemas.microsoft.com/developer/msbuild/2003";
            var node = new XElement(XName.Get("Compile", name.ToString()), new XAttribute("Include", pathToNewFile));

            orginal.Elements().Where(itemGroup => itemGroup.Name.LocalName == "ItemGroup").First(x => x.Elements().Where(f => f.Name.LocalName == "Compile").Count() > 0).Add(node);

            var result = orginal.Elements().Where(itemGroup => itemGroup.Name.LocalName == "ItemGroup");
            var result2 = result.First(x => x.Elements().Where(f => f.Name.LocalName == "Compile").Count() > 0);

            orginal.Save(foundAProjectFile.First());
            return orginal;
        }

        private List<string> GetFilesOfExtension(string path, string extension)
        {
            var files = Directory.GetFiles(path);
            var results = files.OfType<string>().ToList().Where(fileName => fileName.EndsWith(extension));

            return results.ToList();
        }

        private static XElement GetProjectNode(string pathToProjectFile)
        {
            var doc = new XmlDocument();
            doc.Load(pathToProjectFile);
            XElement project = XElement.Load(pathToProjectFile);
            return project;
        }
    }
}
