using Microsoft.VisualStudio.TestTools.UnitTesting;
using CrossZeroAPI;

namespace CrossZeroAPITests {
    [TestClass]
    public class AIStabilityTest {
        [TestMethod]
        public void AIStability() {
            const int iterations = 50;
            for (int i = 0; i < iterations; i++) {
                TestGameProcessor proc = new TestGameProcessor(3, new AI(), new AI(), Marks.Cross);
                proc.Play();
            }
        }
    }
}