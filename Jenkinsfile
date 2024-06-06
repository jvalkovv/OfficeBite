pipeline {
    agent any

    stages {
        stage('Stop Website') {
            steps {
                script {
                    // Stop the specific IIS website using PowerShell
                    powershell 'Stop-WebSite -Name "OfficeBiteProd"'

                    // Ensure IIS process is stopped
                    bat 'iisreset /stop'
                }
            }
        }

        stage('Checkout') {
            steps {
                // Specify the custom workspace directory here
                dir("C:\\Applications\\JenkinsWorkspaces\\OfficeBitePipeline") {
                    // Checkout code from GitHub
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
                    // Stop the service if it's running
                    powershell 'Stop-WebSite -Name "OfficeBiteProd"'
                    bat 'iisreset /stop'

                    // Perform the copy operation here using robocopy
                    bat 'robocopy .\\publish C:\\Applications\\OfficeBiteProd /MIR /Z /R:10 /W:10'
                }
            }
        }

        stage('Start Website') {
            steps {
                script {
                    // Start the specific IIS website using PowerShell
                    powershell 'Start-WebSite -Name "OfficeBiteProd"'
                    bat 'iisreset /start'
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
