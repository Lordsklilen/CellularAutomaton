using System;
using System.Threading;
using System.Windows.Controls;
using CellularAutomaton;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Xunit;

namespace WpfVisualisation
{

    public class DrawingHelperTests
    {
        [WpfTheory]
        [InlineData(0, 0, 1, 1)]
        [InlineData(0, 0, 10, 20)]
        [InlineData(0, 1, 0, 21)]
        [InlineData(1, 0, 11, 0)]
        [InlineData(1, 1, 11, 21)]
        [InlineData(4, 3, 44, 65)]
        [InlineData(9, 4, 99, 99)]
        public void GetPositionTest(int ResY, int ResX,int Pos1,int Pos2 )
        {
            //Arrange
            var img = Substitute.For<Image>();
            img.Width = 100;
            img.Height = 100;
            DrawingHelper dh = new DrawingHelper(img, null, null, 10, 5);
            var result = new System.Drawing.Point(ResX,ResY);
            //Act
            var pos = dh.GetPosition(Pos1, Pos2);
            //Assert
            pos.Should().Be(result);
        }
    }
}
