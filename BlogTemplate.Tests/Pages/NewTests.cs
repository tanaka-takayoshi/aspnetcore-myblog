using My_Blog.Models;
using My_Blog.Pages;
using My_Blog.Services;
using My_Blog.Tests.Fakes;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Xunit;

namespace My_Blog.Tests.Pages
{
    public class NewTests
    {
        [Fact]
        public void OnPostPublish_NoExcerptIsEntered_AutoGenerateExcerpt()
        {
            IFileSystem fakeFileSystem = new FakeFileSystem();
            BlogDataStore testDataStore = new BlogDataStore(fakeFileSystem);
            ExcerptGenerator testExcerptGenerator = new ExcerptGenerator(5);
            SlugGenerator testSlugGenerator = new SlugGenerator(testDataStore);

            NewModel model = new NewModel(testDataStore, testSlugGenerator, testExcerptGenerator);
            model.PageContext = new PageContext();
            model.NewPost = new NewModel.NewPostViewModel {
                Title = "Title",
                Body = "This is the body",
            };

            model.OnPostPublish();

            Assert.Equal("This is the body", model.NewPost.Body);
            Assert.Equal("This ...", model.NewPost.Excerpt);
        }

        [Fact]
        public void OnPostSaveDraft_NoExcerptIsEntered_AutoGenerateExcerpt()
        {
            IFileSystem fakeFileSystem = new FakeFileSystem();
            BlogDataStore testDataStore = new BlogDataStore(fakeFileSystem);
            ExcerptGenerator testExcerptGenerator = new ExcerptGenerator(5);
            SlugGenerator testSlugGenerator = new SlugGenerator(testDataStore);

            NewModel model = new NewModel(testDataStore, testSlugGenerator, testExcerptGenerator);
            model.PageContext = new PageContext();
            model.NewPost = new NewModel.NewPostViewModel {
                Title = "Title",
                Body = "This is the body",
            };

            model.OnPostSaveDraft();

            Assert.Equal("This is the body", model.NewPost.Body);
            Assert.Equal("This ...", model.NewPost.Excerpt);
        }
    }
}
