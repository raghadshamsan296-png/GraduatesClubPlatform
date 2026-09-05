import 'package:flutter/material.dart';
import 'alumni_screen.dart';
import 'events_screen.dart';
import 'home_screen.dart';
import 'jobs_screen.dart';
import '../widgets/ui_helpers.dart';

class AppShell extends StatefulWidget {
  const AppShell({super.key});
  @override
  State<AppShell> createState() => _AppShellState();
}

class _AppShellState extends State<AppShell> {
  int index = 0;
  final pages = const [HomeScreen(), AlumniScreen(), EventsScreen(), JobsScreen()];
  final titles = const ['الرئيسية', 'الخريجون', 'الفعاليات', 'الوظائف'];

  void select(int value) {
    setState(() => index = value);
    if (Scaffold.maybeOf(context)?.isDrawerOpen ?? false) Navigator.pop(context);
  }

  @override
  Widget build(BuildContext context) => Scaffold(
    appBar: AppBar(
      title: Text(titles[index]),
      centerTitle: true,
      actions: [IconButton(onPressed: () => showAboutSheet(context), icon: const Icon(Icons.info_outline), tooltip: 'عن التطبيق')],
    ),
    drawer: Drawer(
      child: SafeArea(child: Column(children: [
        const DrawerHeader(child: Column(mainAxisAlignment: MainAxisAlignment.center, children: [Icon(Icons.school, size: 54), SizedBox(height: 8), Text('نادي الخريجين', style: TextStyle(fontSize: 20, fontWeight: FontWeight.bold))])),
        _drawerItem(0, Icons.home, 'الرئيسية'),
        _drawerItem(1, Icons.people, 'الخريجون'),
        _drawerItem(2, Icons.event, 'الفعاليات'),
        _drawerItem(3, Icons.work, 'الوظائف'),
        const Spacer(),
        ListTile(leading: const Icon(Icons.info), title: const Text('عن التطبيق'), onTap: () { Navigator.pop(context); showAboutSheet(context); }),
      ])),
    ),
    body: IndexedStack(index: index, children: pages),
    bottomNavigationBar: NavigationBar(
      selectedIndex: index,
      onDestinationSelected: (value) => setState(() => index = value),
      destinations: const [
        NavigationDestination(icon: Icon(Icons.home_outlined), selectedIcon: Icon(Icons.home), label: 'الرئيسية'),
        NavigationDestination(icon: Icon(Icons.people_outline), selectedIcon: Icon(Icons.people), label: 'الخريجون'),
        NavigationDestination(icon: Icon(Icons.event_outlined), selectedIcon: Icon(Icons.event), label: 'الفعاليات'),
        NavigationDestination(icon: Icon(Icons.work_outline), selectedIcon: Icon(Icons.work), label: 'الوظائف'),
      ],
    ),
  );

  Widget _drawerItem(int value, IconData icon, String title) => ListTile(
    selected: index == value,
    leading: Icon(icon),
    title: Text(title),
    onTap: () { Navigator.pop(context); setState(() => index = value); },
  );
}
