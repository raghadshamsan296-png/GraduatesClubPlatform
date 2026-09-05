@echo off
title Graduates Club Flutter
cd /d "%~dp0"
where flutter >nul 2>nul
if errorlevel 1 (
  echo Flutter is not installed or is not added to PATH.
  echo Install Flutter, restart VS Code, then run this file again.
  pause
  exit /b 1
)
if not exist "android\gradlew.bat" flutter create --platforms=android,web .
flutter pub get
flutter run
pause
