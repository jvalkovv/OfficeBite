pipeline {
    agent any

    stages {
      stage('Stop IIS') {
            steps {
                script {
                    // Check if the service is running before attempting to stop it
                    if (isServiceRunning('W3SVC')) {
                        bat 'net stop W3SVC'
                    } else {
                        echo 'The World Wide Web Publishing Service is not running.'
                    }
                }
            }
        }
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

        stage('Copy Files') {
            steps {
                script {
                    // Perform the copy operation here using xcopy or robocopy
                    bat 'xcopy /s /y .\\publish C:\\Applications\\OfficeBiteProd'
                }
            }
        }

        stage('Start IIS') {
            steps {
                script {
                    bat 'net start w3svc'
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
