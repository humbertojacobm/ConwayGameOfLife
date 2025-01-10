using NUnit.Framework;
using Moq;
using System;
using System.Threading.Tasks;
using ConwayGameOfLife.Core;
using ConwayGameOfLife.DatabaseModels;
using ConwayGameOfLife.DTO;
using ConwayGameOfLife.Infrastructure.Repository;
using AutoMapper;

namespace ConwayGameOfLife.Tests.Core
{
    [TestFixture]
    public class GameOfLifeServiceTests
    {
        private Mock<IBoardStateRepository> _mockRepo;
        private Mock<IMapper> _mockMapper;
        private GameOfLifeService _service;

        [SetUp]
        public void SetUp()
        {
            _mockRepo = new Mock<IBoardStateRepository>();
            _mockMapper = new Mock<IMapper>();

            _service = new GameOfLifeService(_mockRepo.Object, _mockMapper.Object);
        }

        [Test]
        public async Task UploadBoardStateAsync_ShouldCreateNewBoard_WithGuidId()
        {
            // Arrange
            var boardDto = new BoardDTO
            {
                Width = 5,
                Height = 5,
            };

            var mappedBoard = new Board
            {
                Width = 5,
                Height = 5
            };

            _mockMapper
                .Setup(m => m.Map<Board>(boardDto))
                .Returns(mappedBoard);

            _mockRepo
                .Setup(r => r.CreateBoardAsync(It.IsAny<Board>()))
                .Returns(Task.CompletedTask);

            // Act
            var resultId = await _service.UploadBoardStateAsync(boardDto);

            // Assert
            Assert.That(resultId, Is.Not.EqualTo(Guid.Empty), "Board ID should not be empty");

            _mockRepo.Verify(r => r.CreateBoardAsync(It.Is<Board>(b => b.Width == 5 && b.Height == 5)), Times.Once);
        }

        [Test]
        public async Task GetNextStateAsync_WhenBoardNotFound_ShouldThrowArgumentException()
        {
            // Arrange
            var boardId = Guid.NewGuid();

            _mockRepo
                .Setup(r => r.GetBoardAsync(boardId))
                .ReturnsAsync((Board?)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _service.GetNextStateAsync(boardId);
            });

            Assert.That(ex!.Message, Does.Contain("Board not found"));
        }

        [Test]
        public async Task GetNextStateAsync_WhenBoardIsStable_ShouldNotIncrementStep()
        {

            // Arrange
            var boardId = Guid.NewGuid();
            var existingBoard = new Board
            {
                Id = boardId,
                Width = 3,
                Height = 3,
                Step = 10,
                Cells = new bool[3, 3]
                {
            { false, false, false},
            { false, false, false},
            { false, false, false}
                }
            };

            _mockRepo
                .Setup(r => r.GetBoardAsync(boardId))
                .ReturnsAsync(existingBoard);

            _mockMapper
                .Setup(m => m.Map<BoardDTO>(It.IsAny<Board>()))
                .Returns((Board b) => new BoardDTO
                {
                    Id = b.Id,
                    Width = b.Width,
                    Height = b.Height,
                    Step = b.Step,
                    Cells = b.Cells is null
                        ? null
                        : ConvertToJagged(b.Cells)
                });

            // Act
            var boardDto = await _service.GetNextStateAsync(boardId);

            // Assert
            Assert.That(boardDto.Step, Is.EqualTo(10), "Step should remain the same for a stable board.");

            _mockRepo.Verify(r => r.SaveBoardAsync(It.IsAny<Board>()), Times.Never);
        }

        [Test]
        public async Task GetNextStateAsync_WhenBoardChanges_ShouldIncrementStepAndSave()
        {
            // Arrange
            var boardId = Guid.NewGuid();
            var existingBoard = new Board
            {
                Id = boardId,
                Width = 3,
                Height = 3,
                Step = 10,
                Cells = new bool[3, 3]
                {
            { false, true, false },
            { false, true, false },
            { false, false, false }
                }
            };

            _mockRepo
                .Setup(r => r.GetBoardAsync(boardId))
                .ReturnsAsync(existingBoard);

            _mockMapper
                .Setup(m => m.Map<BoardDTO>(It.IsAny<Board>()))
                .Returns((Board b) => new BoardDTO
                {
                    Id = b.Id,
                    Width = b.Width,
                    Height = b.Height,
                    Step = b.Step,
                    Cells = ConvertToJagged(b.Cells)
                });

            // Act
            var boardDto = await _service.GetNextStateAsync(boardId);

            // Assert
            Assert.That(boardDto.Step, Is.EqualTo(11), "Step should increment if board changes.");
            _mockRepo.Verify(r => r.SaveBoardAsync(It.Is<Board>(b => b.Step == 11)), Times.Once);
        }

        private bool[][] ConvertToJagged(bool[,] twoD)
        {
            int height = twoD.GetLength(0);
            int width = twoD.GetLength(1);
            var jagged = new bool[height][];

            for (int r = 0; r < height; r++)
            {
                jagged[r] = new bool[width];
                for (int c = 0; c < width; c++)
                {
                    jagged[r][c] = twoD[r, c];
                }
            }
            return jagged;
        }
    }
}
