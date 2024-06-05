pipeline {
    agent any
    
    environment {
        DOTNET_CLI_HOME = "C:\\Program Files\\dotnet"
    }

    stages {
        stage('Checkout') {
            steps {
                // Specify the custom workspace directory here
                dir("C:\\Applications\\JenkinsWorkspaces\\OfficeBitePipeline") {
                    // Checkout code from GitHub using the specified SSH credentials
                    checkout([$class: 'GitSCM', 
                              branches: [[name: '*/master']], 
                              doGenerateSubmoduleConfigurations: false, 
                              extensions: [], 
                              submoduleCfg: [], 
                              userRemoteConfigs: [[url: 'https://github.com/jvalkovv/OfficeBite.git']]
                    ])
                }
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
                    def destination = "C:\\Applications\\OfficeBiteProd"
                    if (!fileExists(destination)) {
                        bat "mkdir \"${destination}\""
                    }

                    // Copy published files to IIS directory
                    def source = ".\\publish"
                    bat "xcopy /s /y ${source} ${destination}"
                }
            }
        }
    }

    post {
        success {
            echo 'Build, test, publish, and deploy successful!'
        }
    }
}
