using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Prism.Regions;
using InstaSport.Data.Models;
using InstaSport.Services.Data;
using InstaSport.WPF.ViewModels;
using InstaSport.WPF.Models;
using Telerik.Windows.Controls.Map;
using Location = InstaSport.Data.Models.Location;
using InstaSport.WPF.Views;
using System.Windows.Controls;
using Telerik.Windows.Controls;

namespace InstaSport.Tests.UnitTests
{
    [TestFixture]
    public class LocationsViewModelTests
    {
        private Mock<ILocationsService> locationsServiceMock;
        private Mock<IRegionManager> regionManagerMock;
        private LocationsViewModel viewModel;

        [SetUp]
        public void SetUp()
        {
            // Mock the ILocationsService
            locationsServiceMock = new Mock<ILocationsService>();
            locationsServiceMock.Setup(s => s.GetAll()).Returns(GetTestLocations().AsQueryable());

            // Mock the IRegionManager
            regionManagerMock = new Mock<IRegionManager>();

            // Initialize the LocationsViewModel
            viewModel = new LocationsViewModel(locationsServiceMock.Object, regionManagerMock.Object);
        }

        [Test]
        public void Constructor_ShouldInitializeProperties()
        {
            // Assert
            Assert.IsNotNull(viewModel.Locations);
            Assert.IsNotNull(viewModel.MapItems);
            Assert.IsNotNull(viewModel.CenterLocation);
            Assert.IsNotNull(viewModel.SelectedLocation);
        }

        [Test]
        public void SelectPinCommand_ShouldUpdateSelectedLocationAndCenterLocation()
        {
            // Arrange
            var location = viewModel.MapItems.First().Location;

            // Act
            viewModel.SelectPinCommand.Execute(location);

            // Assert
            Assert.AreEqual(location, viewModel.SelectedLocation.Location);
            Assert.AreEqual(location, viewModel.CenterLocation);
        }

        [Test]
        public void SelectCenterCommand_ShouldUpdateCenterLocation()
        {
            // Arrange
            var mapItem = viewModel.MapItems.First();
            var selectionChangedEventArgs = new SelectionChangedEventArgs(Telerik.Windows.Controls.Primitives.ItemsControlSelector.SelectionChangedEvent, new List<object> { }, new List<object> { mapItem });

            // Act
            viewModel.SelectCenterCommand.Execute(selectionChangedEventArgs);

            // Assert
            Assert.AreEqual(mapItem.Location, viewModel.CenterLocation);
        }

        [Test]
        public void FilterGamesByLocationCommand_ShouldNavigateToGamesView()
        {
            // Arrange
            var mapItem = viewModel.MapItems.First();

            // Act
            viewModel.FilterGamesByLocationCommand.Execute(mapItem);

            // Assert
            regionManagerMock.Verify(r => r.RequestNavigate("MainRegion", nameof(GamesView), It.IsAny<NavigationParameters>()), Times.Once);
        }

        private IEnumerable<Location> GetTestLocations()
        {
            return new List<Location>
            {
                new Location
                {
                    Id = 1,
                    Name = "Location 1",
                    Latitude = 40.712776m,
                    Longitude = -74.005974m,
                    CityId = 1,
                    City = new City { Id = 1, Name = "City 1" }
                },
                new Location
                {
                    Id = 2,
                    Name = "Location 2",
                    Latitude = 34.052235m,
                    Longitude = -118.243683m,
                    CityId = 2,
                    City = new City { Id = 2, Name = "City 2" }
                },
                new Location
                {
                    Id = 3,
                    Name = "Location 3",
                    Latitude = 51.507351m,
                    Longitude = -0.127758m,
                    CityId = 3,
                    City = new City { Id = 3, Name = "City 3" }
                }
            };
        }
    }
}
