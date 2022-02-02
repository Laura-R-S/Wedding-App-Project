using System;
using System.IO;
using Microsoft.ML;
using Microsoft.ML.Trainers;

namespace ConsoleApp5 {
    class Program {


            //Training data
            static IDataView trainingDataView;

            //Testing data
            static IDataView testDataView;
        
            static void Main(string[] args) {

                //instantiating machine learning context to create components for data preparation, feature engineering, training, prediction, model evaluation. 
                MLContext mlContext = new MLContext(); 

                //calling LoadData method 
                (IDataView trainingDataView, IDataView testDataView) = LoadData(mlContext);

                ITransformer model = BuildAndTrainModel(mlContext, trainingDataView);

                EvaluateModel(mlContext, testDataView, model);

                PredictingRecommendation(mlContext, model);

                SaveModel(mlContext, trainingDataView.Schema, model);

            }

            public static (IDataView training, IDataView test) LoadData(MLContext mlContext) {

                //saving path of data to variable 
                var ratingsDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "ratingsRand.csv");

                //read data from text file using TextLoader 
                IDataView allData = mlContext.Data.LoadFromTextFile<PicRating>(ratingsDataPath, hasHeader: true, separatorChar: ',');

                //split data
                DataOperationsCatalog.TrainTestData splitData = mlContext.Data.TrainTestSplit(allData, testFraction: 0.2, seed: 1);
                trainingDataView = splitData.TrainSet;
                testDataView = splitData.TestSet;

                return (trainingDataView, testDataView);


            }

            // method to transform the data 
            public static ITransformer BuildAndTrainModel(MLContext mlContext, IDataView trainingDataView) {

                IEstimator<ITransformer> estimator = mlContext.Transforms.Conversion
                    .MapValueToKey(outputColumnName: "userIdEncoded", inputColumnName: "userID2")
                    .Append(mlContext.Transforms.Conversion
                    .MapValueToKey(outputColumnName: "venIdEncoded", inputColumnName: "venID"));


                var options = new MatrixFactorizationTrainer.Options {
                    MatrixColumnIndexColumnName = "userIdEncoded",
                    MatrixRowIndexColumnName = "venIdEncoded",
                    LabelColumnName = "Label",
                    NumberOfIterations = 20,
                    ApproximationRank = 100
                };

                var trainerEstimator = estimator.Append(mlContext.Recommendation().Trainers.MatrixFactorization(options));

                // training the model
                ITransformer model = trainerEstimator.Fit(trainingDataView);

                return model;

            }

            // Evaluating the model 
            public static void EvaluateModel(MLContext mlContext, IDataView testDataView, ITransformer model) {

                var prediction = model.Transform(testDataView);
                var metrics = mlContext.Regression.Evaluate(prediction, labelColumnName: "Label", scoreColumnName: "Score");
            

                Console.WriteLine("Rmse : " + metrics.RootMeanSquaredError.ToString());
                Console.WriteLine("R^2: " + metrics.RSquared.ToString());

            }

            //Prediction method 
            public static void PredictingRecommendation(MLContext mlContext, ITransformer model)
            {

                var predictionEngine = mlContext.Model.CreatePredictionEngine<PicRating, RatingPrediction>(model);

                    var testInput = new PicRating()
                    {
                        userID2 = 9,
                        venID = 3
                    };

                    var RatingPrediction = predictionEngine.Predict(testInput);


                    if (Math.Round(RatingPrediction.Score, 1) > 3)
                    {

                        Console.WriteLine("Venue " + testInput.venID + " is recommended for user " + testInput.userID2);
                    }
                    else
                    {
                        Console.WriteLine("Venue " + testInput.venID + " is not recommended for user " + testInput.userID2);
                    }
                    Console.WriteLine("Score " + RatingPrediction.Score);
            }
        


            //save model 
            public static void SaveModel(MLContext mlContext, DataViewSchema trainingDataViewSchema, ITransformer model) {

                var modelPath = Path.Combine(Environment.CurrentDirectory, "Data", "Consolepic5.zip");

                mlContext.Model.Save(model, trainingDataViewSchema, modelPath);

            }
        }
    }
