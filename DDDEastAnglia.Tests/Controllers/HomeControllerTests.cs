using DDDEastAnglia.Controllers;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Helpers.Agenda;
using DDDEastAnglia.Helpers.Sessions;
using NSubstitute;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Controllers
{
    [TestFixture]
    public sealed class HomeControllerTests
    {
        [Test]
        public void Agenda_ShouldRedirectBackToIndex_WhenTheAgendaCannotBePublished()
        {
            var conferenceLoader = new ConferenceLoaderBuilder()
                                        .WithAgendaNotPublished()
                                        .Build();
            var homeController = CreateHomeController(conferenceLoader);

            var viewName = homeController.Agenda().GetRedirectionViewName();

            Assert.That(viewName, Is.EqualTo("Index"));
        }

        [Test]
        public void Accommodation_ShouldRedirectBackToIndex_WhenRegistrationIsNotOpen()
        {
            var conferenceLoader = new ConferenceLoaderBuilder()
                                        .WithRegistrationNotOpen()
                                        .Build();
            var homeController = CreateHomeController(conferenceLoader);

            var viewName = homeController.Agenda().GetRedirectionViewName();

            Assert.That(viewName, Is.EqualTo("Index"));
        }

        [Test]
        public void Register_ShouldRedirectBackToIndex_WhenRegistrationIsNotOpen()
        {
            var conferenceLoader = new ConferenceLoaderBuilder()
                                        .WithRegistrationNotOpen()
                                        .Build();
            var homeController = CreateHomeController(conferenceLoader);

            var viewName = homeController.Agenda().GetRedirectionViewName();

            Assert.That(viewName, Is.EqualTo("Index"));
        }

        [Test]
        public void Preview_ShouldRedirectToTheHomePage_WhenTheConferenceIsNotInPreview()
        {
            var conferenceLoader = new ConferenceLoaderBuilder()
                                        .NotInPreview()
                                        .Build();
            var homeController = CreateHomeController(conferenceLoader);

            var result = homeController.Preview();

            Assert.That(result.GetRedirectionUrl(), Is.EqualTo("~/"));
        }

        [Test]
        public void Closed_ShouldRedirectToTheHomePage_WhenTheConferenceIsNotClosed()
        {
            var conferenceLoader = new ConferenceLoaderBuilder()
                                        .WhenNotClosed()
                                        .Build();
            var homeController = CreateHomeController(conferenceLoader);

            var result = homeController.Closed();

            Assert.That(result.GetRedirectionUrl(), Is.EqualTo("~/"));
        }

        private HomeController CreateHomeController(IConferenceLoader conferenceLoader)
        {
            var sponsorModelQuery = new AllPublicSponsors(new InMemorySponsorRepository(), new DefaultSponsorSorter());
            return new HomeController(conferenceLoader, sponsorModelQuery, new AgendaSessionsLoader(Substitute.For<ISessionLoader>(), Substitute.For<ISpeakerRepository>()));
        }
    }
}
