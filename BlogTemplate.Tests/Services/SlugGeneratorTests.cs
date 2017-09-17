using My_Blog.Models;
using My_Blog.Services;
using My_Blog.Tests.Fakes;
using Xunit;

namespace My_Blog.Tests.Services
{
    public class SlugGeneratorTests
    {
        [Fact]
        public void CreateSlug_ReplacesSpaces()
        {
            IFileSystem testFileSystem = new FakeFileSystem();
            BlogDataStore testDataStore = new BlogDataStore(testFileSystem);
            SlugGenerator testSlugGenerator = new SlugGenerator(testDataStore);
            Post test = new Post
            {
                Title = "Test title"
            };
            test.Slug = testSlugGenerator.CreateSlug(test.Title);

            Assert.Equal(test.Slug, "Test-title");
        }

        [Theory]
        [InlineData("test?", "test")]
        [InlineData("test<", "test")]
        [InlineData("test>", "test")]
        [InlineData("test/", "test")]
        [InlineData("test&", "test")]
        [InlineData("test!", "test")]
        [InlineData("test#", "test")]
        [InlineData("test''", "test")]
        [InlineData("test|", "test")]
        [InlineData("test©", "test")]
        [InlineData("test%", "test")]
        public void CreateSlug_TitleContainsInvalidChars_RemoveInvalidCharsInSlug(string input, string expected)
        {
            BlogDataStore testDataStore = new BlogDataStore(new FakeFileSystem());
            SlugGenerator testSlugGenerator = new SlugGenerator(testDataStore);

            Assert.Equal(expected, testSlugGenerator.CreateSlug(input));
        }
    }
}
