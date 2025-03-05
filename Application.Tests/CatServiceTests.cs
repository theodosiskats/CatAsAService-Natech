using Application.Interfaces;
using Application.Interfaces.RepositoryInterfaces;
using Application.Services;
using Domain.Entities;
using Models.InfrastructureModels;
using Moq;

namespace Application.Tests
{
    [TestFixture]
    public class CatServiceTests
    {
        // Mocks
        private Mock<ICatApiClient> _catApiClientMock;
        private Mock<IUnitOfWork> _uowMock;
        private Mock<ICatsRepository> _catsRepositoryMock;
        private Mock<ITagsRepository> _tagsRepositoryMock;

        // System Under Test
        private CatService _catService;
        private static readonly string[] ExpectedTags = ["Playful", "Friendly", "Curious", "Intelligent", "Alert"];

        [SetUp]
        public void SetUp()
        {
            // Initialize mocks
            _catApiClientMock = new Mock<ICatApiClient>();
            _uowMock = new Mock<IUnitOfWork>();
            _catsRepositoryMock = new Mock<ICatsRepository>();
            _tagsRepositoryMock = new Mock<ITagsRepository>();

            // Setup UoW to return mock repositories
            _uowMock.Setup(u => u.CatsRepository).Returns(_catsRepositoryMock.Object);
            _uowMock.Setup(u => u.TagsRepository).Returns(_tagsRepositoryMock.Object);

            // Create CatService with mocks
            _catService = new CatService(_catApiClientMock.Object, _uowMock.Object);
        }

        #region GetCatById Tests

        [Test]
        public async Task GetCatById_ShouldReturnCat_WhenCatExists()
        {
            // Arrange
            int catId = 123;
            var catEntity = new Cat { Id = catId, CatId = "Abc123" };

            _catsRepositoryMock
                .Setup(repo => repo.GetByIdAsync(catId))
                .ReturnsAsync(catEntity);

            // Act
            var result = await _catService.GetCatById(catId);

            // Assert
            Assert.NotNull(result);
            Assert.That(result, Is.EqualTo(catEntity));
            _catsRepositoryMock.Verify(repo => repo.GetByIdAsync(catId), Times.Once);
        }

        [Test]
        public async Task GetCatById_ShouldReturnNull_WhenCatDoesNotExist()
        {
            // Arrange
            int catId = 999;
            _catsRepositoryMock
                .Setup(repo => repo.GetByIdAsync(catId))
                .ReturnsAsync((Cat)null!);

            // Act
            var result = await _catService.GetCatById(catId);

            // Assert
            Assert.IsNull(result);
            _catsRepositoryMock.Verify(repo => repo.GetByIdAsync(catId), Times.Once);
        }

        #endregion

        #region FetchCats Tests

        [Test]
        public async Task FetchCats_ShouldFetchAndProcessAndSave_WhenCalled()
        {
            // Arrange
            var catApiResponse = new List<CatApiResponse>
            {
                new CatApiResponse
                {
                    Id = "cat123",
                    Url = "http://example.com/cat.jpg",
                    Width = 200,
                    Height = 300,
                    Breeds =
                    [
                        new Breed
                        {
                            Temperament = "Playful, Friendly",
                            Name = "TestBreed",
                            Weight = new Weight
                            {
                                Imperial = "7 - 10",
                                Metric = "3 - 5"
                            }
                        }
                    ]
                }
            };

            // Setup catApiClient to return data
            _catApiClientMock
                .Setup(client => client.FetchRandomCats())
                .ReturnsAsync(catApiResponse);

            // We don't care about the final cats saved [we test the processing], so let's just pass them through
            _catsRepositoryMock
                .Setup(repo => repo.SaveCatsRangeAndImagesAsync(It.IsAny<List<Cat>>()))
                .ReturnsAsync((List<Cat> cats) => cats);

            // Setup empty existing cats
            _catsRepositoryMock
                .Setup(repo => repo.GetByIdsAsync(It.IsAny<List<string>>()))
                .ReturnsAsync([]); 

            // Setup empty existing tags
            _tagsRepositoryMock
                .Setup(repo => repo.GetByNamesAsync(It.IsAny<List<string>>()))
                .ReturnsAsync([]);

            // Act
            var result = await _catService.FetchCats();

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, result.Count);                // We only fetched/processed 1 cat
            Assert.AreEqual("cat123", result[0].CatId);      // The mapped catId
            Assert.AreEqual(2, result[0].Tags.Count);        // "Playful" & "Friendly"
            Assert.AreEqual("Playful", result[0].Tags[0].Name);

            // Verify calls
            _catApiClientMock.Verify(client => client.FetchRandomCats(), Times.Once);
            _catsRepositoryMock.Verify(repo => repo.SaveCatsRangeAndImagesAsync(It.IsAny<List<Cat>>()), Times.Once);
        }

        #endregion

        #region ProcessRawFetchedCats Tests

        [Test]
        public async Task ProcessRawFetchedCats_ShouldFilterOutExistingCats()
        {
            // Arrange
            var apiCats = new List<CatApiResponse>
            {
                new() { Id = "newCat1" },
                new() { Id = "existingCat" },
                new() { Id = "newCat2" }
            };

            // Pretend we already have "existingCat" in DB
            var existingCats = new List<Cat>
            {
                new() { CatId = "existingCat" }
            };

            // We also have some random existing tags
            var existingTags = new List<Tag>
            {
                new() { Name = "Calm" }
            };

            // Setup
            _catsRepositoryMock
                .Setup(repo => repo.GetByIdsAsync(It.Is<List<string>>(ids => ids.Contains("existingCat"))))
                .ReturnsAsync(existingCats);

            _tagsRepositoryMock
                .Setup(repo => repo.GetByNamesAsync(It.IsAny<List<string>>()))
                .ReturnsAsync(existingTags);

            // Act
            var processedCats = await _catService.ProcessRawFetchedCats(apiCats);

            // Assert
            Assert.That(processedCats, Has.Count.EqualTo(2)); // "newCat1" and "newCat2" remain
            Assert.IsTrue(processedCats.All(c => c.CatId.StartsWith("newCat")));
        }

        [Test]
        public async Task ProcessRawFetchedCats_ShouldHandleBreedsAndTemperaments()
        {
            // Arrange
            var apiCats = new List<CatApiResponse>
            {
                new()
                {
                    Id = "cat1",
                    Breeds =
                    [
                        new Breed()
                        {
                            Temperament = "Playful, Friendly, Curious",
                            Name = "TestBreed1"
                        },

                        new Breed
                        {
                            Temperament = "Intelligent, Alert",
                            Name = "TestBreed2"
                        }
                    ]
                }
            };

            // No existing cats, no existing tags
            _catsRepositoryMock
                .Setup(repo => repo.GetByIdsAsync(It.IsAny<List<string>>()))
                .ReturnsAsync([]);
            _tagsRepositoryMock
                .Setup(repo => repo.GetByNamesAsync(It.IsAny<List<string>>()))
                .ReturnsAsync([]);

            // Act
            var processedCats = await _catService.ProcessRawFetchedCats(apiCats);

            // Assert
            // cat1 has two breeds:
            //   - First breed: "Playful, Friendly, Curious"
            //   - Second breed: "Intelligent, Alert"
            // => total distinct tags: "Playful", "Friendly", "Curious", "Intelligent", "Alert" = 5
            Assert.AreEqual(1, processedCats.Count);
            Assert.AreEqual("cat1", processedCats[0].CatId);
            Assert.AreEqual(5, processedCats[0].Tags.Count);
            CollectionAssert.AreEquivalent(
                ExpectedTags,
                (processedCats[0].Tags ?? []).Select(t => t.Name)
            );
        }

        #endregion

        #region GetCatsAsync Tests

        [Test]
        public async Task GetCatsAsync_ShouldCallRepositoryWithCorrectParameters()
        {
            // Arrange
            string? tag = "playful";
            int page = 2;
            int pageSize = 5;

            var catList = new List<Cat>
            {
                new() { CatId = "cat1" },
                new() { CatId = "cat2" }
            };

            _catsRepositoryMock
                .Setup(repo => repo.GetCatsAsync(tag, page, pageSize))
                .ReturnsAsync(catList);

            // Act
            var result = await _catService.GetCatsAsync(tag, page, pageSize);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count(), Is.EqualTo(2));
            _catsRepositoryMock.Verify(repo => repo.GetCatsAsync(tag, page, pageSize), Times.Once);
        }

        #endregion
    }
}
