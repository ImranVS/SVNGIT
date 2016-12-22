var webpack = require('webpack');
var HtmlWebpackPlugin = require('html-webpack-plugin');
var ExtractTextPlugin = require('extract-text-webpack-plugin');
var helpers = require('./helpers');
var tsConfigPath = helpers.root('tsconfig.webpack.json');

module.exports = {
    entry: {
        'app': './scripts/main.ts'
    },

    resolve: {
        extensions: ['', '.ts', '.min.js', '.js']
    },

    module: {
        preLoaders: [
            {
                test: /\.ts$/,
                loader: 'string-replace',
                query: {
                    multiple: [
                        {
                            search: "templateUrl: '/partial/",
                            replace: "template: '/partial/",
                            flags: 'gi'
                        },
                        {
                            search: "templateUrl: '.*/([^']+)'",
                            replace: "templateUrl: './$1'",
                            flags: 'gi'
                        }
                    ]
                }
            },
        ],
        loaders: [
            {
                test: /\.ts$/,
                loaders: [
                    'awesome-typescript-loader?tsconfig=' + tsConfigPath,
                    'angular2-template-loader'
                ],
            },
            {
                test: /\.html$/,
                loader: 'html'
            }
        ],
        postLoaders: [
            {
                test: /\.ts$/,
                loader: 'string-replace',
                query: {
                    search: "template: '/partial/",
                    replace: "templateUrl: '/partial/",
                    flags: 'gi'
                }
            }
        ]
    }
};
