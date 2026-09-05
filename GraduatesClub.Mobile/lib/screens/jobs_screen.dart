import 'package:flutter/material.dart';
import '../models/api_models.dart';
import '../services/api_service.dart';
import '../widgets/ui_helpers.dart';

class JobsScreen extends StatefulWidget {
  const JobsScreen({super.key});
  @override State<JobsScreen> createState() => _JobsScreenState();
}

class _JobsScreenState extends State<JobsScreen> {
  List<JobPosting> items = const [];
  bool loading = true;
  String? error;
  String query = '';
  String company = 'الكل';
  List<String> get companies { final values = {for (final x in items) if (x.companyName.isNotEmpty) x.companyName}.toList()..sort(); return ['الكل', ...values]; }
  List<JobPosting> get filtered => items.where((x) { final q = query.trim().toLowerCase(); return (q.isEmpty || x.jobTitle.toLowerCase().contains(q) || x.requirements.toLowerCase().contains(q)) && (company == 'الكل' || x.companyName == company); }).toList();

  @override void initState() { super.initState(); load(); }
  Future<void> load() async {
    setState(() { loading = true; error = null; });
    try { final data = await ApiService().getJobs(); if (mounted) setState(() { items = data; loading = false; }); }
    catch (_) { if (mounted) setState(() { items = const [JobPosting(id: 1, jobTitle: 'مطور تطبيقات', companyName: 'شركة التقنية', requirements: 'Flutter وREST API')]; loading = false; error = 'تعذر الاتصال؛ تظهر بيانات تجريبية.'; }); }
  }

  @override Widget build(BuildContext context) => LoadingOrError(
    loading: loading, error: error, onRetry: load,
    child: Column(children: [
      Padding(padding: const EdgeInsets.all(16), child: Row(children: [
        Expanded(flex: 2, child: TextField(decoration: const InputDecoration(prefixIcon: Icon(Icons.search), labelText: 'ابحث بالمسمى أو المتطلبات', border: OutlineInputBorder()), onChanged: (v) => setState(() => query = v))),
        const SizedBox(width: 8),
        Expanded(child: DropdownButtonFormField<String>(value: company, decoration: const InputDecoration(labelText: 'الشركة', border: OutlineInputBorder()), items: companies.map((x) => DropdownMenuItem(value: x, child: Text(x, overflow: TextOverflow.ellipsis))).toList(), onChanged: (v) => setState(() => company = v ?? 'الكل'))),
      ])),
      Expanded(child: RefreshIndicator(onRefresh: load, child: filtered.isEmpty
        ? ListView(children: const [SizedBox(height: 100), Center(child: Text('لا توجد نتائج مطابقة'))])
        : ListView.builder(padding: const EdgeInsets.all(16), itemCount: filtered.length, itemBuilder: (_, i) { final x = filtered[i]; return Card(margin: const EdgeInsets.only(bottom: 10), child: Padding(padding: const EdgeInsets.all(16), child: Column(crossAxisAlignment: CrossAxisAlignment.start, children: [Text(x.jobTitle, style: const TextStyle(fontSize: 18, fontWeight: FontWeight.bold)), const SizedBox(height: 4), Text(x.companyName, style: TextStyle(color: Theme.of(context).colorScheme.primary)), const Divider(), Text(x.requirements)]))); }))),
    ]),
  );
}
