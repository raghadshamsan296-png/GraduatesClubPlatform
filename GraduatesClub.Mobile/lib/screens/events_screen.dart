import 'package:flutter/material.dart';
import '../models/api_models.dart';
import '../services/api_service.dart';
import '../widgets/ui_helpers.dart';

class EventsScreen extends StatefulWidget {
  const EventsScreen({super.key});
  @override State<EventsScreen> createState() => _EventsScreenState();
}

class _EventsScreenState extends State<EventsScreen> {
  List<ClubEvent> items = const [];
  bool loading = true;
  String? error;
  String query = '';
  int? year;
  List<int> get years => ({for (final x in items) x.eventDate.year}.toList()..sort((a, b) => b.compareTo(a)));
  List<ClubEvent> get filtered => items.where((x) {
    final q = query.trim().toLowerCase();
    return (q.isEmpty || x.title.toLowerCase().contains(q) || x.description.toLowerCase().contains(q) || (x.alumniName ?? '').toLowerCase().contains(q)) && (year == null || x.eventDate.year == year);
  }).toList();

  @override void initState() { super.initState(); load(); }
  Future<void> load() async {
    setState(() { loading = true; error = null; });
    try { final data = await ApiService().getEvents(); if (mounted) setState(() { items = data; loading = false; }); }
    catch (_) { if (mounted) setState(() { items = [ClubEvent(id: 1, title: 'ملتقى الخريجين', description: 'لقاء سنوي لخريجي الجامعة', eventDate: DateTime(2026, 12, 15), alumniName: 'أحمد محمد')]; loading = false; error = 'تعذر الاتصال؛ تظهر بيانات تجريبية.'; }); }
  }

  @override Widget build(BuildContext context) => LoadingOrError(
    loading: loading, error: error, onRetry: load,
    child: Column(children: [
      Padding(padding: const EdgeInsets.all(16), child: Row(children: [
        Expanded(flex: 2, child: TextField(decoration: const InputDecoration(prefixIcon: Icon(Icons.search), labelText: 'ابحث عن فعالية', border: OutlineInputBorder()), onChanged: (v) => setState(() => query = v))),
        const SizedBox(width: 8),
        Expanded(child: DropdownButtonFormField<int?>(value: year, decoration: const InputDecoration(labelText: 'السنة', border: OutlineInputBorder()), items: [const DropdownMenuItem<int?>(value: null, child: Text('الكل')), ...years.map((x) => DropdownMenuItem<int?>(value: x, child: Text('$x')))], onChanged: (v) => setState(() => year = v))),
      ])),
      Expanded(child: RefreshIndicator(onRefresh: load, child: filtered.isEmpty
        ? ListView(children: const [SizedBox(height: 100), Center(child: Text('لا توجد نتائج مطابقة'))])
        : ListView.builder(padding: const EdgeInsets.all(16), itemCount: filtered.length, itemBuilder: (_, i) { final x = filtered[i]; return Card(margin: const EdgeInsets.only(bottom: 10), child: ListTile(leading: const CircleAvatar(child: Icon(Icons.event)), title: Text(x.title), subtitle: Text('${x.eventDate.year}/${x.eventDate.month}/${x.eventDate.day}\n${x.description}'), isThreeLine: true)); }))),
    ]),
  );
}
