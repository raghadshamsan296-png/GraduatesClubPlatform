import 'package:flutter/material.dart';

class LoadingOrError extends StatelessWidget {
  final bool loading;
  final String? error;
  final VoidCallback onRetry;
  final Widget child;
  const LoadingOrError({super.key, required this.loading, required this.error, required this.onRetry, required this.child});

  @override
  Widget build(BuildContext context) {
    if (loading) return const Center(child: CircularProgressIndicator());
    return Column(children: [
      if (error != null) MaterialBanner(
        content: Text(error!),
        leading: const Icon(Icons.cloud_off),
        actions: [TextButton(onPressed: onRetry, child: const Text('إعادة المحاولة'))],
      ),
      Expanded(child: child),
    ]);
  }
}

void showAboutSheet(BuildContext context) {
  showModalBottomSheet(
    context: context,
    showDragHandle: true,
    builder: (_) => const Padding(
      padding: EdgeInsets.fromLTRB(24, 8, 24, 32),
      child: Column(mainAxisSize: MainAxisSize.min, children: [
        CircleAvatar(radius: 32, child: Icon(Icons.school, size: 34)),
        SizedBox(height: 12),
        Text('منصة نادي الخريجين', style: TextStyle(fontSize: 20, fontWeight: FontWeight.bold)),
        SizedBox(height: 8),
        Text('تطبيق موحّد لمتابعة الخريجين والفعاليات والفرص الوظيفية.', textAlign: TextAlign.center),
      ]),
    ),
  );
}
