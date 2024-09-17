using Xunit;public class ScoringTests{
[Fact]public void ThresholdIsStrict(){double delta=.4*9.80665*3.6;var score=Scoring.Calculate(new Trip(100,new[]{new Sample(0,delta,100),new Sample(1,0,100)}));Assert.Equal(0,score.Braking);}
[Fact]public void BrakingAndClamp(){var score=Scoring.Calculate(new Trip(1,new[]{new Sample(0,100,50),new Sample(1,0,50)}));Assert.Equal(1,score.Braking);Assert.Equal(0,score.Value);}
[Fact]public void InvalidTime(){Assert.Throws<ArgumentException>(()=>Scoring.Calculate(new Trip(1,new[]{new Sample(0,0,50),new Sample(0,1,50)})));}
}