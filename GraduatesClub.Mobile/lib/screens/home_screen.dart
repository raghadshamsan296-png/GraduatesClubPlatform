import 'package:flutter/material.dart';
import '../services/api_service.dart';

class HomeScreen extends StatefulWidget {
  const HomeScreen({super.key});
  @override
  State<HomeScreen> createState() => _HomeScreenState();
}

class _HomeScreenState extends State<HomeScreen> {
  int alumni = 0, events = 0, jobs = 0;
  bool loading = true;
  String status = 'جاري الاتصال بالخادم...';

  @override
  void initState() { super.initState(); load(); }

  Future<void> load() async {
    setState(() { loading = true; status = 'جاري الاتصال بالخادم...'; });
    try {
      final api = ApiService();
      final results = await Future.wait([api.getAlumni(), api.getEvents(), api.getJobs()]);
      if (!mounted) return;
      setState(() { alumni = results[0].length; events = results[1].length; jobs = results[2].length; status = 'متصل بالـAPI'; loading = false; });
    } catch (_) {
      if (!mounted) return;
      setState(() { alumni = 3; events = 2; jobs = 2; status = 'وضع العرض - شغّل API لجلب البيانات الحقيقية'; loading = false; });
    }
  }

  @override
  Widget build(BuildContext context) => RefreshIndicator(
    onRefresh: load,
    child: ListView(padding: const EdgeInsets.all(16), children: [
      Container(
        padding: const EdgeInsets.all(24),
        decoration: BoxDecoration(gradient: const LinearGradient(colors: [Color(0xFF073763), Color(0xFF0B4F8A)]), borderRadius: BorderRadius.circular(24)),
        child: const Column(crossAxisAlignment: CrossAxisAlignment.start, children: [Text('مرحبًا بك في نادي الخريجين', style: TextStyle(color: Colors.white, fontSize: 24, fontWeight: FontWeight.bold)), SizedBox(height: 8), Text('تواصل، شارك، واصنع فرصتك القادمة.', style: TextStyle(color: Colors.white70))]),
      ),
      const SizedBox(height: 12),
      Row(children: [Icon(loading ? Icons.sync : status.startsWith('متصل') ? Icons.cloud_done : Icons.cloud_off, color: status.startsWith('متصل') ? Colors.green : Colors.orange), const SizedBox(width: 8), Expanded(child: Text(status)), IconButton(onPressed: load, icon: const Icon(Icons.refresh))]),
      const SizedBox(height: 12),
      GridView.count(
        crossAxisCount: 2, shrinkWrap: true, physics: const NeverScrollableScrollPhysics(), crossAxisSpacing: 12, mainAxisSpacing: 12, childAspectRatio: 1.3,
        children: [_stat('الخريجون', alumni, Icons.people, Colors.blue), _stat('الفعاليات', events, Icons.event, Colors.purple), _stat('الوظائف', jobs, Icons.work, Colors.green), _stat('الأقسام', 3, Icons.account_tree, Colors.orange)],
      ),
    ]),
  );

  Widget _stat(String label, int value, IconData icon, Color color) => Card(child: Padding(padding: const EdgeInsets.all(16), child: Column(mainAxisAlignment: MainAxisAlignment.center, children: [Icon(icon, color: color, size: 34), const SizedBox(height: 6), Text('$value', style: const TextStyle(fontSize: 24, fontWeight: FontWeight.bold)), Text(label)])));
}
