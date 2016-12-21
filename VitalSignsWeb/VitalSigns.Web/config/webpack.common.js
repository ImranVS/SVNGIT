var webpack = require('webpack');
var HtmlWebpackPlugin = require('html-webpack-plugin');
var ExtractTextPlugin = require('extract-text-webpack-plugin');
var helpers = require('./helpers');

module.exports = {
    entry: {
        'app': './scripts/main.ts'
    },

    resolve: {
        extensions: ['', '.ts', '.min.js', '.js']
    },

    module: {
        loaders: [
          {
              test: /\.ts$/,
              loaders: ['awesome-typescript-loader', 'angular2-template-loader'],
          },
          {
              test: /\.html$/,
              loader: 'html'
          }
        ]
    }
};
