using NUnit.Framework;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using TailSpin.SpaceGame.Web.Models;
using System;
using System.Linq.Expressions;
using System.Linq;
using TailSpin.SpaceGame.Web;

namespace mslearn_tailspin_spacegame_web
{
    [TestFixture]
    public class RegionTest
    {
        private LocalDocumentDBRepository<Score> _scoreRepository;
        [SetUp]
        public void Setup()
        {
            _scoreRepository = new LocalDocumentDBRepository<Score>(@"SampleData/scores.json");
        }
        [TestCase("Milky Way")]
        [TestCase("Andromeda")]
        [TestCase("Pinwheel")]
        [TestCase("NGC 1300")]
        [TestCase("Messier 82")]
        public void FetchOnlyRequestedGameRegion(string gameRegion)
        {
            const int PAGE = 0; // take the first page of results
            const int MAX_RESULTS = 10; // sample up to 10 results

            // Form the query predicate.
            // This expression selects all scores for the provided game region.
            Expression<Func<Score, bool>> queryPredicate = score => (score.GameRegion == gameRegion);

            // Fetch the scores.
            Task<IEnumerable<Score>> scoresTask = _scoreRepository.GetItemsAsync(
                queryPredicate, // the predicate defined above
                score => 1, // we don't care about the order
                PAGE,
                MAX_RESULTS
            );
            IEnumerable<Score> scores = scoresTask.Result;

            // Verify that each score's game region matches the provided game region.
            Assert.That(scores, Is.All.Matches<Score>(score => score.GameRegion == gameRegion));
        }
        [Test]
        public void FetchOnlyRequestedGameRegionAll()
        {
            FetchOnlyRequestedGameRegion("Milky Way");
            FetchOnlyRequestedGameRegion("Andromeda");
            FetchOnlyRequestedGameRegion("Pinwheel");
            FetchOnlyRequestedGameRegion("NGC 1300");
            FetchOnlyRequestedGameRegion("Messier 82");
        }
    }
}