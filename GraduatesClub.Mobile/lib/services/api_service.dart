import 'dart:convert';
import 'package:flutter/foundation.dart';
import 'package:http/http.dart' as http;
import '../models/api_models.dart';

class ApiService {
  static String get baseUrl {
    const configured = String.fromEnvironment('API_BASE_URL');
    if (configured.isNotEmpty) return configured;
    return !kIsWeb && defaultTargetPlatform == TargetPlatform.android
        ? 'http://10.0.2.2:5158'
        : 'http://localhost:5158';
  }

  Future<List<dynamic>> _getList(String path) async {
    final response = await http.get(Uri.parse('$baseUrl$path')).timeout(const Duration(seconds: 5));
    if (response.statusCode != 200) throw Exception('HTTP ${response.statusCode}');
    return jsonDecode(utf8.decode(response.bodyBytes)) as List<dynamic>;
  }

  Future<List<Alumni>> getAlumni() async => (await _getList('/api/Alumni')).map((x) => Alumni.fromJson(x as Map<String, dynamic>)).toList();
  Future<List<ClubEvent>> getEvents() async => (await _getList('/api/Event')).map((x) => ClubEvent.fromJson(x as Map<String, dynamic>)).toList();
  Future<List<JobPosting>> getJobs() async => (await _getList('/api/Jobs')).map((x) => JobPosting.fromJson(x as Map<String, dynamic>)).toList();
}
