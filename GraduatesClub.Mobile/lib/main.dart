
import 'package:flutter/material.dart';
import 'screens/app_shell.dart';

void main() => runApp(const GraduatesClubApp());

class GraduatesClubApp extends StatelessWidget {
  const GraduatesClubApp({super.key});

  @override
  Widget build(BuildContext context) {
    const primary = Color(0xFF0B4F8A);
    return MaterialApp(
      debugShowCheckedModeBanner: false,
      title: 'نادي الخريجين',
      theme: ThemeData(
        useMaterial3: true,
        fontFamily: 'DejaVu',
        colorScheme: ColorScheme.fromSeed(seedColor: primary),
        scaffoldBackgroundColor: const Color(0xFFF4F7FB),
        cardTheme: const CardThemeData(elevation: 1, margin: EdgeInsets.zero),
      ),
      builder: (context, child) => Directionality(
        textDirection: TextDirection.rtl,
        child: child ?? const SizedBox.shrink(),
      ),
      home: const AppShell(),
    );
  }
}
