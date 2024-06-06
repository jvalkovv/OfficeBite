pipeline {
    agent any
    environment {
        DOTNET_CLI_HOME = "C:\\Program Files\\dotnet"
    }

    stages {

    stage('Stop Website') {
            steps {
                script {
                    // Stop the specific IIS website
                    bat 'C:\\Windows\\System32\\inetsrv\\appcmd stop site /site.name:"OfficeBiteProd"'

                       // Stop the application pool
                    bat 'C:\\Windows\\System32\\inetsrv\\appcmd stop apppool /apppool.name:"OfficeBiteProd"'
                }
            }
        }
        stage('Checkout') {
            steps {
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

             stage('Start Website') {
            steps {
                script {
                    // Start the specific IIS website
                   bat 'C:\\Windows\\System32\\inetsrv\\appcmd start site /site.name:"OfficeBiteProd"'

                          // Start the application pool
                    bat 'C:\\Windows\\System32\\inetsrv\\appcmd start apppool /apppool.name:"OfficeBiteProd"'
            
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
