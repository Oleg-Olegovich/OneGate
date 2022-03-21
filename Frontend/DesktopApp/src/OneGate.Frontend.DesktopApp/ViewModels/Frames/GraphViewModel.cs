﻿using System.Collections.ObjectModel;
using System.Drawing;
using ReactiveUI;
using ScottPlot.Avalonia;
using ScottPlot;
using ScottPlot.Plottable;
using System.Collections.Generic;
using OneGate.Backend.Gateway.User.Api.Contracts.Timeseries;
using OneGate.Frontend.Client;

namespace OneGate.Frontend.DesktopApp.ViewModels.Frames
{
    public class GraphViewModel : ViewModelBase
    {
        private Plot _plot;

        private List<GraphLayer> _layers;

        private OHLC[] _ohlc;

        private string[] _tickLabels;

        private double[] _tickPositions;

        private ObservableCollection<OhlcModel> _ohlcData;
        public ObservableCollection<OhlcModel> OhlcData
        {
            get => _ohlcData;
            set
            {
                this.RaiseAndSetIfChanged(ref _ohlcData, value);
                ConvertToTickLabels(_ohlcData);
                _ohlc = ConvertToScottPlotOhlc(_ohlcData);
            }
        }

        private AvaPlot _graph = new AvaPlot();
        public AvaPlot Graph
        {
            get => _graph;
            set => this.RaiseAndSetIfChanged(ref _graph, value);
        }

        /// <summary>
        /// Creates an empty graph.
        /// </summary>
        public GraphViewModel() { }

        /// <summary>
        /// Creates an OHLC graph.
        /// </summary>
        public GraphViewModel(ObservableCollection<OhlcModel> data)
        {
            OhlcData = data;
            _layers = new List<GraphLayer>();
            DrawOhlcGraph(_ohlc);
        }

        /// <summary>
        /// Creates a custom graph plot (for the application style).
        /// </summary>
        private void CreatePlot()
        {
            _plot = new Plot();
            var background = ColorTranslator.FromHtml("#FF13152E");
            var grid = ColorTranslator.FromHtml("#2d2d2d");
            var tick = ColorTranslator.FromHtml("#FFFFFFFF");
            var axisLabel = ColorTranslator.FromHtml("#FFFFFFFF");
            var titleLabel = ColorTranslator.FromHtml("#FFFFFFFF");
            _plot.Style(background, background, grid, tick, axisLabel, titleLabel);
            // Temporary code.
            _plot.Title("The random graph");
            _plot.YLabel("Stock Price (USD)");
            _plot.XTicks(_tickPositions, _tickLabels);
            Graph.Reset(_plot);
        }

        /// <summary>
        /// Draws a Point graph of a close data set.
        /// </summary>
        private void DrawPointGraph()
            => DrawPointGraph(ConvertToPointGraphCoordinates(OhlcData));

        /// <summary>
        /// Draws a Point graph of a specific data set.
        /// </summary>
        private void DrawPointGraph(double[][] coordinates)
        {
            if (_plot == null)
            {
                CreatePlot();
            }
            _plot.AddScatter(coordinates[0],
                coordinates[1], Color.Red, 3);
        }

        /// <summary>
        /// Draws a OHLC graph of a specific data set.
        /// </summary>
        private void DrawOhlcGraph(OHLC[] data)
        {
            if (_plot == null)
            {
                CreatePlot();
            }
            _plot.AddCandlesticks(data);
        }

        /// <summary>
        /// Adds a new layer to the current graph.
        /// </summary>
        public void AddLayer(GraphLayer layer)
        {
            _layers.Add(layer);
            DrawPointGraph(layer.Data);
            // In the future, the legend will change here.
        }

        /// <summary>
        /// Removes the layer of the current graph.
        /// </summary>
        public void RemoveLayer(GraphLayer layer)
        {
            var index = _layers.IndexOf(layer);
            if (_plot == null
                || index == -1)
            {
                return;
            }
            RemoveLayer(_plot.GetPlottables()[index + 1]);
            _layers.Remove(layer);
        }

        private void RemoveLayer(IPlottable layer)
            => _plot.Remove(layer);

        /// <summary>
        /// Converts the OhlcSeries collection to an array of data for AvaPlot.
        /// </summary>
        public static OHLC[] ConvertToScottPlotOhlc(ObservableCollection<OhlcModel> data)
        {
            var ohlcs = new OHLC[data.Count];
            for (int i = 0; i < data.Count; ++i)
            {
                ohlcs[i] = ConvertToScottPlotOhlc(data[i]);
            }
            return ohlcs;
        }

        /// <summary>
        /// Converts an instance of OhlcSeries to a data instance for AvaPlot.
        /// </summary>
        public static OHLC ConvertToScottPlotOhlc(OhlcModel data)
            => new OHLC(data.Open, data.High, data.Low, data.Close, data.Timestamp.ToOADate());

        /// <summary>
        /// Converts the OhlcSeries collection to an array of tick labels.
        /// This is a description of the abscissa axis.
        /// </summary>
        private void ConvertToTickLabels(ObservableCollection<OhlcModel> data)
        {
            _tickLabels = new string[data.Count / 7];
            _tickPositions = new double[data.Count / 7];
            for (int i = 0, j = 0; i < _tickLabels.Length && j < data.Count; ++i, j += 7)
            {
                _tickLabels[i] = ConvertToTickLabels(data[j]);
                _tickPositions[i] = data[j].Timestamp.ToOADate();
            }
        }

        /// <summary>
        /// Converts an instance of OhlcSeries to a tick label.
        /// </summary>
        public static string ConvertToTickLabels(OhlcModel data)
            => data.Timestamp.ToString().Split(' ')[0];

        /// <summary>
        /// Converts OHLC data to the coordinates of the graph points by Close.
        /// </summary>
        public static double[][] ConvertToPointGraphCoordinates(ObservableCollection<OhlcModel> data)
            => new double[][] { ConvertToArrayOfX(data), ConvertToArrayOfY(data) };

        public static double[] ConvertToArrayOfX(ObservableCollection<OhlcModel> data)
        {
            var xS = new double[data.Count];
            for (int i = 0; i < data.Count; ++i)
            {
                xS[i] = data[i].Timestamp.ToOADate();
            }
            return xS;
        }

        public static double[] ConvertToArrayOfY(ObservableCollection<OhlcModel> data)
        {
            var yS = new double[data.Count];
            for (int i = 0; i < data.Count; ++i)
            {
                yS[i] = data[i].Close;
            }
            return yS;
        }
    }
}
