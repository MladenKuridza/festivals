using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Mladen_Kuridza.Controllers;
using Mladen_Kuridza.Interfaces;
using Mladen_Kuridza.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Mladen_Kuridza_Test.ControllerTest
{
    public class FestivalRepositoryTest
    {
        [Fact]
        public void GetFestival_ValidId_ReturnsObject()
        {
            Festival festival = new Festival()
            {
                Id = 1,
                Naziv = "Exit",
                Cena = 11000,
                Godina = 1997,
                MestoId = 1,
                Mesto = new Mesto() { Id = 1, Naziv = "Novi Sad" }
            };

            FestivalDTO festivalDto = new FestivalDTO()
            {
                Id = 1,
                Naziv = "Exit",
                MestoNaziv = "Novi Sad",
                Godina = 1997,
                Cena = 11000
            };

            var mockRepository = new Mock<IFestivalRepository>();
            mockRepository.Setup(x => x.GetById(1)).Returns(festival);
            var mockRepository2 = new Mock<IMestoRepository>();

            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new FestivalProfile()));
            IMapper mapper = new Mapper(mapperConfiguration);

            var controller = new FestivaliController(mockRepository.Object, mapper, mockRepository2.Object);

            // Act
            var actionResult = controller.Festival(1) as OkObjectResult;

            // Assert
            Assert.NotNull(actionResult);
            Assert.NotNull(actionResult.Value);

            FestivalDTO dtoResult = (FestivalDTO)actionResult.Value;

            Console.WriteLine(dtoResult);

            Assert.Equal(festivalDto, dtoResult);
        }


        [Fact]
        public void GetFestival_InvalidId_ReturnsNotFound()
        {
            Festival festival = new Festival()
            {
                Id = 1,
                Naziv = "Exit",
                Cena = 11000,
                Godina = 1997,
                MestoId = 1,
                Mesto = new Mesto() { Id = 1, Naziv = "Novi Sad" }
            };

            FestivalDTO festivalDto = new FestivalDTO()
            {
                Id = 1,
                Naziv = "Exit",
                MestoNaziv = "Novi Sad",
                Godina = 1997,
                Cena = 11000
            };

            var mockRepository = new Mock<IFestivalRepository>();
            mockRepository.Setup(x => x.GetById(1)).Returns(festival);
            var mockRepository2 = new Mock<IMestoRepository>();

            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new FestivalProfile()));
            IMapper mapper = new Mapper(mapperConfiguration);

            var controller = new FestivaliController(mockRepository.Object, mapper, mockRepository2.Object);

            // Act
            var actionResult = controller.Festival(12) as NotFoundObjectResult;

            // Assert
            Assert.Null(actionResult);
        }

        [Fact]
        public void DeleteFestival_ValidId_ReturnsNoContent()
        {
            // Arrange
            Festival festival = new Festival()
            {
                Id = 1,
                Naziv = "Exit",
                Cena = 11000,
                Godina = 1997,
                MestoId = 1,
                Mesto = new Mesto() { Id = 1, Naziv = "Novi Sad" }
            };


            var mockRepository = new Mock<IFestivalRepository>();
            mockRepository.Setup(x => x.GetById(1)).Returns(festival);
            var mockRepository2 = new Mock<IMestoRepository>();

            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new FestivalProfile()));
            IMapper mapper = new Mapper(mapperConfiguration);

            var controller = new FestivaliController(mockRepository.Object, mapper, mockRepository2.Object);

            // Act
            var actionResult = controller.DeleteFestival(1) as NoContentResult;

            // Assert
            Assert.NotNull(actionResult);
        }

        [Fact]
        public void PostFestival_ValidRequest()
        {
            // Arrange
            Festival festival = new Festival()
            {
                Id = 1,
                Naziv = "Exit",
                Cena = 11000,
                Godina = 1997,
                MestoId = 1,
                Mesto = new Mesto() { Id = 1, Naziv = "Novi Sad" }
            };

            var mockRepository = new Mock<IFestivalRepository>();
            mockRepository.Setup(x => x.GetById(1)).Returns(festival);
            var mockRepository2 = new Mock<IMestoRepository>();

            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new FestivalProfile()));
            IMapper mapper = new Mapper(mapperConfiguration);


            var controller = new FestivaliController(mockRepository.Object, mapper, mockRepository2.Object);

            // Act
            var actionResult = controller.PostFestival(festival) as CreatedAtActionResult;

            // Assert
            Assert.NotNull(actionResult);

            Assert.Equal("Festival", actionResult.ActionName);
            Assert.Equal(1, actionResult.RouteValues["id"]);
            Assert.NotNull(actionResult.Value);
            Assert.Equal(festival, actionResult.Value);
        }
    }
}
