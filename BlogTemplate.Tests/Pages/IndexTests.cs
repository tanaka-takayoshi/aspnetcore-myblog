using System;
using System.Collections.Generic;
using System.Linq;
using My_Blog.Models;
using My_Blog.Pages;
using My_Blog.Tests.Fakes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Xunit;

namespace My_Blog.Tests.Pages
{
    public class IndexTests
    {
        [Fact]
        public void Index_PostSummary_DoesNotIncludeDeletedComments()
        {
            BlogDataStore testDataStore = new BlogDataStore(new FakeFileSystem());
            testDataStore.SavePost(new Post {
                Comments = new List<Comment> {
                    new Comment {
                        Body = "Test comment 1",
                        IsPublic = true,
                    },
                    new Comment {
                        Body = "Deleted comment 1",
                        IsPublic = false,
                    },
                },
                IsPublic = true,
                PubDate = DateTimeOffset.Now,
            });

            IndexModel model = new IndexModel(testDataStore);
            model.OnGet();

            Assert.Equal(1, model.PostSummaries.Count());
            Assert.Equal(1, model.PostSummaries.First().CommentCount);
        }
    }
}
