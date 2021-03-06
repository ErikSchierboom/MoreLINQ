#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2008 Jonathan Skeet. All rights reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

namespace MoreLinq.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public class TopByTests
    {
        [Test]
        public void TopByWithNullSequence()
        {
            Assert.AreEqual("source", Assert.Throws<ArgumentNullException>(() => MoreEnumerable.TopBy<object, object>(null, 0, e => e)).ParamName);
            Assert.AreEqual("source", Assert.Throws<ArgumentNullException>(() => MoreEnumerable.TopBy<object, object>(null, 0, e => e, Comparer<object>.Default)).ParamName);
        }

        [Test]
        public void TopBy()
        {
            var ns = MoreEnumerable.RandomDouble().Take(10).ToArray();

            const int count = 5;
            var top = ns.Select((n, i) => KeyValuePair.Create(i, n))
                        .Reverse()
                        .TopBy(count, e => e.Key);

            top.Select(e => e.Value).AssertSequenceEqual(ns.Take(count));
        }

        [Test]
        public void TopWithComparer()
        {
            var alphabet = Enumerable.Range(0, 26)
                                     .Select((n, i) => ((char)((i % 2 == 0 ? 'A' : 'a') + n)).ToString())
                                     .ToArray();

            var ns = alphabet.Zip(MoreEnumerable.RandomDouble(), KeyValuePair.Create).ToArray();
            var top = ns.TopBy(5, e => e.Key, StringComparer.Ordinal);

            top.Select(e => e.Key[0]).AssertSequenceEqual('A', 'C', 'E', 'G', 'I');
        }

        [Test]
        public void TopByIsLazy()
        {
            new BreakingSequence<object>().TopBy(1, o => o);
        }
    }
}