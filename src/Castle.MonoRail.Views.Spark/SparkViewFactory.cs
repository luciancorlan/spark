﻿// Copyright 2004-2008 Castle Project - http://www.castleproject.org/
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.


namespace Castle.MonoRail.Views.Spark
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Castle.MonoRail.Framework;

    using global::Spark;
    using global::Spark.FileSystem;
    using IViewSource = global::Spark.FileSystem.IViewSource;

    public class SparkViewFactory : ViewEngineBase, IViewFolder
    {
        public override void Service(IServiceProvider provider)
        {
            base.Service(provider);

            Engine = (ISparkViewEngine)provider.GetService(typeof(ISparkViewEngine));
            if (Engine == null)
                Engine = new SparkViewEngine(typeof(SparkView).FullName, this);
        }

        public ISparkViewEngine Engine { get; set; }

        public override string ViewFileExtension
        {
            get { return "xml"; }
        }

        public override bool HasTemplate(string templateName)
        {
            return base.HasTemplate(Path.ChangeExtension(templateName, ViewFileExtension));
        }

        public override void Process(string templateName, TextWriter output, IEngineContext context, IController controller,
                                     IControllerContext controllerContext)
        {
            var viewName = Path.GetFileName(templateName);
            var location = Path.GetDirectoryName(templateName);

            string masterName = null;
            if (controllerContext.LayoutNames != null)
                masterName = string.Join(" ", controllerContext.LayoutNames);

            var view = (SparkView)Engine.CreateInstance(location, viewName, masterName);
            view.Contextualize(context, controllerContext);
            output.Write(view.RenderView());
        }

        public override void Process(string templateName, string layoutName, TextWriter output,
                                     IDictionary<string, object> parameters)
        {
            throw new NotImplementedException();
        }

        public override void ProcessPartial(string partialName, TextWriter output, IEngineContext context,
                                            IController controller, IControllerContext controllerContext)
        {
            throw new NotImplementedException();
        }

        public override void RenderStaticWithinLayout(string contents, IEngineContext context, IController controller,
                                                      IControllerContext controllerContext)
        {
            throw new NotImplementedException();
        }

        public override bool SupportsJSGeneration
        {
            get { return false; }
        }

        public override string JSGeneratorFileExtension
        {
            get { return null; }
        }

        public override object CreateJSGenerator(JSCodeGeneratorInfo generatorInfo, IEngineContext context,
                                                 IController controller, IControllerContext controllerContext)
        {
            throw new NotImplementedException();
        }

        public override void GenerateJS(string templateName, TextWriter output, JSCodeGeneratorInfo generatorInfo,
                                        IEngineContext context, IController controller, IControllerContext controllerContext)
        {
            throw new NotImplementedException();
        }

        IList<string> IViewFolder.ListViews(string path)
        {
            return ViewSourceLoader.ListViews(path);
        }

        bool IViewFolder.HasView(string path)
        {
            return ViewSourceLoader.HasSource(path);
        }

        IViewSource IViewFolder.GetViewSource(string path)
        {
            return new ViewSource(ViewSourceLoader.GetViewSource(path));
        }

        private class ViewSource : IViewSource
        {
            private readonly Framework.IViewSource _source;

            public ViewSource(Framework.IViewSource source)
            {
                _source = source;
            }

            public long LastModified
            {
                get { return _source.LastModified; }
            }

            public Stream OpenViewStream()
            {
                return _source.OpenViewStream();
            }
        }

    }
}
