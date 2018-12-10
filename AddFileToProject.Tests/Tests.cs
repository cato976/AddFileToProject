using System.IO;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using Should;
using System.Linq;
using System;
using System.Xml.Linq;
using System.Collections;

namespace AddFileToProject.Tests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void Can_Get_Project()
        {
            var projectManager = new ProjectManager();
            var answer = projectManager.GetProjectPath(Directory.GetParent(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)).Parent.FullName);

            var currentProjectFile = Directory.GetParent(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)).Parent.FullName;
            currentProjectFile = GetFilesOfExtension(currentProjectFile, "csproj").First();
            answer.ShouldEqual(currentProjectFile);
        }

        [Test]
        public void Can_Find_ItemGroup_For_CSFiles()
        {
            var projectManager = new ProjectManager();
            var currentProjectFile = Directory.GetParent(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)).Parent.FullName;
            var answer = projectManager.GetPathToCompiledFiles(GetFilesOfExtension(currentProjectFile, "csproj").First());

            Console.WriteLine(answer);
            var item = (XElement)answer.FirstNode;
            item.FirstAttribute.Value.ShouldEqual("Tests.cs");
        }

        [Test]
        [Ignore("This test modifies the current project file")]
        public void AddFileToProject()
        {
            var projectManager = new ProjectManager();
            var currentProjectFile = Directory.GetParent(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)).Parent.FullName;

            var newNode = projectManager.AddFileToProject(currentProjectFile + "\\", GetFilesOfExtension(currentProjectFile, "cs").First());
            newNode.ShouldNotBeNull();
        }

        [Test]
        [Ignore("This test modifies the current project file")]
        public void AddNestedFileToProject()
        {
            var projectManager = new ProjectManager();
            var currentProjectFile = Directory.GetParent(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)).Parent.FullName;
            List<string> sub = new List<string>();
            sub.Add("\\Test\\");

            currentProjectFile = currentProjectFile + string.Concat(sub[0]);
            var newNode = projectManager.AddFileToProject(currentProjectFile, GetFilesOfExtension(currentProjectFile, "cs").First());
            newNode.ShouldNotBeNull();
        }

        private List<string> GetFilesOfExtension(string path, string extension)
        {
            var files = Directory.GetFiles(path);
            var results = files.OfType<string>().ToList().Where(fileName => fileName.EndsWith(extension));

            return results.ToList();
        }
    }
}
