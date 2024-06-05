pipeline {
    agent any

    environment {
        DOTNET_CLI_HOME = "C:\\Program Files\\dotnet"
    }

    stages {
        stage('Checkout') {
            steps {
                checkout scm
            }
        }

        stage('Build') {
            steps {
                script {
                    // Restoring dependencies
                    bat "dotnet restore"

                    // Building the application
                    bat "dotnet build --configuration Release"
                }
            }
        }

        stage('Test') {
            steps {
                script {
                    // Running tests
                    bat "dotnet test --no-restore --configuration Release"
                }
            }
        }

        stage('Publish') {
            steps {
                script {
                    // Publishing the application
                    bat "dotnet publish --no-restore --configuration Release --output .\\publish"
                }
            }
        }

        stage('Deploy to IIS') {
            steps {
                script {
                    // Create the destination directory if it doesn't exist
                    def destination = "D:\\Applications\\OfficeBite"
                    bat "mkdir \"${destination}\""

                    // Copy published files to IIS directory
                    def source = ".\\publish"
                    bat "xcopy /s /y ${source} ${destination}"
                }
            }
        } // <--- Added closing curly brace for 'Deploy to IIS' stage

    } // <--- Added closing curly brace for 'stages' section

    post {
        success {
            echo 'Build, test, publish, and deploy successful!'
        }
    }
}
