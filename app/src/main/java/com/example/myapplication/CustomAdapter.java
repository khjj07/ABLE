package com.example.myapplication;
import android.content.Context;
import android.graphics.drawable.Drawable;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.BaseAdapter;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import com.bumptech.glide.Glide;
import com.bumptech.glide.request.RequestOptions;

import java.util.ArrayList;
import java.util.List;

public class CustomAdapter extends BaseAdapter {

    private Context context;
    // Adapter에 추가된 데이터를 저장하기 위한 ArrayList
    private ArrayList<ListViewItem> listViewItemList = new ArrayList<ListViewItem>() ;


    public CustomAdapter(){

    }

    @Override
    public int getCount() {
        return listViewItemList.size();
    }



    @Override
    public View getView(int position, View convertView, ViewGroup parent) {
        final int pos = position;
        final Context context = parent.getContext();

        if (convertView == null){
            LayoutInflater layoutInflater = LayoutInflater.from(parent.getContext());
            convertView = layoutInflater.inflate(R.layout.row_item, parent, false);
        }

        TextView tv_name = (TextView) convertView.findViewById(R.id.textView_name);
        TextView tv_summary = (TextView) convertView.findViewById(R.id.textView_summary);
        ImageView iv_thumb = (ImageView) convertView.findViewById(R.id.imageView_thumb);

        ListViewItem listViewItem = (ListViewItem) listViewItemList.get(position);

        tv_name.setText(listViewItem.getName());
        tv_summary.setText(listViewItem.getSummary());
        iv_thumb.setImageDrawable(listViewItem.getThumb_url());

        return convertView;
    }

    // 지정한 위치(position)에 있는 데이터와 관계된 아이템(row)의 ID를 리턴. : 필수 구현
    @Override
    public long getItemId(int position) {
        return position ;
    }

    // 지정한 위치(position)에 있는 데이터 리턴 : 필수 구현
    @Override
    public Object getItem(int position) {
        return listViewItemList.get(position) ;
    }

    // 아이템 데이터 추가를 위한 함수. 개발자가 원하는대로 작성 가능.
    public void addItem(Drawable icon, String title, String desc) {
        ListViewItem item = new ListViewItem();

        item.setIcon(icon);
        item.setTitle(title);
        item.setDesc(desc);

        listViewItemList.add(item);
    }
}