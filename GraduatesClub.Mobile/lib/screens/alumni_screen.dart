import 'package:flutter/material.dart';
import '../models/api_models.dart';
import '../services/api_service.dart';
import '../widgets/ui_helpers.dart';

class AlumniScreen extends StatefulWidget {
  const AlumniScreen({super.key});
  @override
  State<AlumniScreen> createState() => _AlumniScreenState();
}

class _AlumniScreenState extends State<AlumniScreen> {
  List<Alumni> items = const [];
  bool loading = true;
  String? error;
  String query = '';
  String department = 'الكل';
  int? year;

  List<String> get departments {
    final values = {for (final x in items) if ((x.departmentName ?? '').isNotEmpty) x.departmentName!}.toList()..sort();
    return ['الكل', ...values];
  }

  List<int> get years => ({for (final x in items) x.graduationYear}.toList()..sort((a, b) => b.compareTo(a)));

  List<Alumni> get filtered => items.where((x) {
    final q = query.trim().toLowerCase();
    final textMatch = q.isEmpty || x.fullName.toLowerCase().contains(q) || x.email.toLowerCase().contains(q) || x.phone.contains(q);
    return textMatch && (department == 'الكل' || x.departmentName == department) && (year == null || x.graduationYear == year);
  }).toList();

  @override
  void initState() { super.initState(); load(); }

  Future<void> load() async {
    setState(() { loading = true; error = null; });
    try {
      final data = await ApiService().getAlumni();
      if (mounted) setState(() { items = data; loading = false; });
    } catch (_) {
      if (mounted) setState(() {
        items = const [Alumni(id: 1, fullName: 'أحمد محمد', email: 'ahmed@example.com', phone: '777123456', graduationYear: 2026, departmentName: 'تقنية المعلومات')];
        loading = false;
        error = 'تعذر الاتصال؛ تظهر بيانات تجريبية.';
      });
    }
  }

  @override
  Widget build(BuildContext context) => LoadingOrError(
    loading: loading,
    error: error,
    onRetry: load,
    child: Column(children: [
      Padding(padding: const EdgeInsets.fromLTRB(16, 16, 16, 8), child: Column(children: [
        TextField(
          decoration: const InputDecoration(prefixIcon: Icon(Icons.search), labelText: 'ابحث بالاسم أو البريد أو الهاتف', border: OutlineInputBorder()),
          onChanged: (value) => setState(() => query = value),
        ),
        const SizedBox(height: 10),
        Row(children: [
          Expanded(child: DropdownButtonFormField<String>(
            value: department,
            decoration: const InputDecoration(labelText: 'القسم', border: OutlineInputBorder()),
            items: departments.map((x) => DropdownMenuItem(value: x, child: Text(x, overflow: TextOverflow.ellipsis))).toList(),
            onChanged: (value) => setState(() => department = value ?? 'الكل'),
          )),
          const SizedBox(width: 8),
          Expanded(child: DropdownButtonFormField<int?>(
            value: year,
            decoration: const InputDecoration(labelText: 'سنة التخرج', border: OutlineInputBorder()),
            items: [const DropdownMenuItem<int?>(value: null, child: Text('كل السنوات')), ...years.map((x) => DropdownMenuItem<int?>(value: x, child: Text('$x')))],
            onChanged: (value) => setState(() => year = value),
          )),
        ]),
      ])),
      Expanded(child: RefreshIndicator(
        onRefresh: load,
        child: filtered.isEmpty
          ? ListView(children: const [SizedBox(height: 100), Center(child: Text('لا توجد نتائج مطابقة'))])
          : ListView.separated(
              padding: const EdgeInsets.all(16),
              itemCount: filtered.length,
              separatorBuilder: (_, __) => const SizedBox(height: 10),
              itemBuilder: (_, i) {
                final x = filtered[i];
                return Card(child: ListTile(
                  leading: CircleAvatar(child: Text(x.fullName.isEmpty ? '؟' : x.fullName[0])),
                  title: Text(x.fullName),
                  subtitle: Text('${x.departmentName ?? 'بدون قسم'} • ${x.graduationYear}\n${x.email}'),
                  isThreeLine: true,
                  trailing: const Icon(Icons.chevron_left),
                ));
              },
            ),
      )),
    ]),
  );
}
