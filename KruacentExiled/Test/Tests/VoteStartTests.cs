using KruacentExiled.Misc.Features.VoteStart;
using KruacentExiled.Test.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Mirror.NetworkLoop;

namespace KruacentExiled.Test.Tests
{
    [TestClass]
    public class VoteStartTests
    {
        private VoteStart _votestart;
        public VoteStartTests()
        {
            _votestart = new VoteStart();
            
        }



        [TestCase(1)]
        [TestCase(2)]
        public void AddVote_Normal(int value)
        {
            _votestart.RemoveAllVote();
            _votestart.AddVote(value);
            if (!_votestart.DidVote(value))
            {
                Assert.Fail("should be in");
            }
        }


        [TestCase(1)]
        [TestCase(2)]
        public void RemoveVote_Normal(int value)
        {
            _votestart.RemoveAllVote();
            _votestart.AddVote(value);

            _votestart.CancelVote(value);

            if (_votestart.DidVote(value))
            {
                Assert.Fail("shouldn't be in");
            }


        }

        /// <summary>
        /// Remove vote when didn't vote
        /// </summary>
        /// <param name="value"></param>
        [TestCase(3)]
        [TestCase(4)]
        public void RemoveVoteDidntVote_ThrowsArgumentOutOfRangeException(int value)
        {
            _votestart.RemoveAllVote();

            Assert.Catch<ArgumentOutOfRangeException>(() =>
            {
                _votestart.CancelVote(value);
            },"should send an exception");
        }

        /// <summary>
        /// Add a vote when already voted
        /// </summary>
        /// <param name="value"></param>
        [TestCase(3)]
        [TestCase(4)]
        public void AddVoteAlreadyVote_ThrowsException(int value)
        {
            _votestart.RemoveAllVote();

            _votestart.AddVote(value);

            Assert.Catch<ArgumentException>(() =>
            {
                _votestart.AddVote(value);
            }, "should send an exception");
        }

        // number vote, min number 
        [TestCase(5,2,true)]
        [TestCase(8,8,true)]
        [TestCase(2, 5,false)]
        [TestCase(8, 69,false)]
        public void CheckVote_NormalCase(int nbVote, int minNumber,bool expected)
        {
            _votestart.RemoveAllVote();

            _votestart.MinimumVote = minNumber;

            Assert.IsFalse(_votestart.CheckVote(), "should be false");

            for (int i = 0; i < nbVote; i++)
            {
                _votestart.AddVote(i);
            }

            Assert.IsTrue(_votestart.CheckVote() == expected, "should be "+ expected);
        }
    }
}
