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
        [InlineData(9, 9, 99, 99)]
        public void GetPositionTest(int ResX, int ResY,int PosX,int PosY )
        {
            //Arrange
            var img = Substitute.For<Image>();
            img.Width = 100;
            img.Height = 100;
            DrawingHelper dh = new DrawingHelper(img, null, null, 10, 10);
            var result = new System.Drawing.Point(ResX, ResY);
            //Act
            var pos = dh.GetPosition(PosX, PosY);
            //Assert
            pos.Should().Be(result);
        }
    }
}
