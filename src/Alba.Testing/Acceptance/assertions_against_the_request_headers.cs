﻿using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Alba.Testing.Acceptance
{
    public class assertions_against_the_request_headers : ScenarioContext
    {
        public class Person
        {
            public string FirstName { get; set; }
        }

        [Fact]
        public Task using_scenario_with_JsonInputIs_should_set_content_type()
        {
            host.Handlers["/one"] = c =>
            {
                c.Response.StatusCode = 200;
                return Task.CompletedTask;
            };

            return host.Scenario(x =>
            {
                x.Post
                    .Json(new { FirstName = "Tom" })
                    .ToUrl("/one");

                x.Context.Request.ContentType.ShouldBe(MimeType.Json.Value);
            });
        }

        [Fact]
        public Task using_scenario_with_JsonInputIs_should_set_content_length()
        {
            host.Handlers["/one"] = c =>
            {
                c.Response.StatusCode = 200;
                return Task.CompletedTask;
            };

            return host.Scenario(x =>
            {
                x.Post
                    .Json(new { FirstName = "Tom" })
                    .ToUrl("/one");
                x.Context.Request.ContentLength.ShouldNotBeNull();
            });
        }

        [Fact]
        public Task using_scenario_with_TextInputIs_should_set_content_type()
        {
            host.Handlers["/one"] = c =>
            {
                c.Response.StatusCode = 200;
                return Task.CompletedTask;
            };

            return host.Scenario(x =>
            {
                x.Post
                    .Text("Hello, world")
                    .ToUrl("/one");
                x.Context.Request.ContentType.ShouldBe(MimeType.Text.Value);
            });
        }

        [Fact]
        public Task using_scenario_with_TextInputIs_should_set_content_length()
        {
            host.Handlers["/one"] = c =>
            {
                c.Response.StatusCode = 200;
                return Task.CompletedTask;
            };

            return host.Scenario(x =>
            {
                x.Post
                    .Text("Hello, world")
                    .ToUrl("/one");
                x.Context.Request.ContentLength.ShouldNotBeNull();
            });
        }

        [Fact]
        public Task using_scenario_with_XmlInputIs_should_set_content_type()
        {
            host.Handlers["/one"] = c =>
            {
                c.Response.StatusCode = 200;
                return Task.CompletedTask;
            };

            return host.Scenario(x =>
            {
                x.Post
                    .Xml(new Person {FirstName = "Tom"}) //xml serializer doesn't support anonymous obj
                    .ToUrl("/one");
                x.Context.Request.ContentType.ShouldBe(MimeType.Xml.Value);
            });
        }

        [Fact]
        public Task using_scenario_with_XmlInputIs_should_set_content_length()
        {
            host.Handlers["/one"] = c =>
            {
                c.Response.StatusCode = 200;
                return Task.CompletedTask;
            };

            return host.Scenario(x =>
            {
                x.Post
                    .Xml(new Person { FirstName = "Tom" })
                    .ToUrl("/one");
                x.Context.Request.ContentLength.ShouldNotBeNull();
            });
        }

        [Fact]
        public Task using_scenario_with_FormDataIs_should_set_content_type()
        {
            host.Handlers["/one"] = c =>
            {
                c.Response.StatusCode = 200;
                return Task.CompletedTask;
            };

            return host.Scenario(x =>
            {
                x.Post
                    .FormData(new Person { FirstName = "Tom" })
                    .ToUrl("/one");
                x.Context.Request.ContentType.ShouldBe(MimeType.HttpFormMimetype);
            });
        }

        [Fact]
        public Task using_scenario_with_FormDataIs_should_set_content_length()
        {
            host.Handlers["/one"] = c =>
            {
                c.Response.StatusCode = 200;
                return Task.CompletedTask;
            };

            return host.Scenario(x =>
            {
                x.Post
                    .FormData(new Person { FirstName = "Tom" })
                    .ToUrl("/one");
                x.Context.Request.ContentLength.ShouldNotBeNull();
            });
        }
    }
}
