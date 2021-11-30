package com.example.myapplication;

import android.content.Intent;
import android.graphics.drawable.Drawable;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.ListView;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.fragment.app.Fragment;

import static androidx.core.content.ContextCompat.getDrawable;


public class MapFragment extends Fragment {

    private View rootView;


    ListView customListView;
    CustomAdapter customAdapter;




    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater,
                             @Nullable ViewGroup container,
                             @Nullable Bundle savedInstanceState) {
        rootView = inflater.inflate(R.layout.fragment_map, container, false);
        // Adapter 생성
        customAdapter = new CustomAdapter() ;
        customListView = (ListView) rootView.findViewById(R.id.listView_custom);

        customListView.setAdapter(customAdapter);

        // 첫 번째 아이템 추가.
        customAdapter.addItem(getDrawable(getContext(), R.drawable.maze),
                "Maze", "주어진 미로를 보고 탈출 방법을 순서에 따라\n블럭으로 나열하는 맵입니다.");
        // 두 번째 아이템 추가.
        customAdapter.addItem(getDrawable(getContext(), R.drawable.hanoi_tower),
                "Hanoi Tower", "순서가 뒤바뀐 원반들을 가장 넓은 원반이 맨\n아래로, 가장 작은 원반이 맨 위로 오도록 순서를\n바꾸는 맵입니다.") ;
        // 세 번째 아이템 추가.
        customAdapter.addItem(getDrawable(getContext(), R.drawable.cube),
                "Rotating Cube", "주어진 큐브의 모양을 보고 같은 모양이 되게\n나의 큐브를 회전시키는 맵입니다.") ;

        customListView.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> parent, View view, int position, long l) {
//                //각 아이템을 분간 할 수 있는 position과 뷰
//                String selectedItem = (String) view.findViewById(R.id.textView_name).getTag().toString();
//                Toast.makeText(getContext(), "Clicked: " + position +" " + selectedItem, Toast.LENGTH_SHORT).show();

                // get item
                ListViewItem item = (ListViewItem) parent.getItemAtPosition(position);


                String titleStr = item.getName() ;
                String descStr = item.getSummary() ;
                Drawable iconDrawable = item.getThumb_url() ;

                //Toast.makeText(getContext(), "Clicked: " + position +"번째 아이템인 " + titleStr + "을 선택하셨습니다. ", Toast.LENGTH_SHORT).show();
                Toast.makeText(getContext(), "Unity 실행 중..\n" + titleStr + "을 선택하셨습니다. ", Toast.LENGTH_SHORT).show();

                Intent intent = new Intent(getActivity(), UnityPlayerActivity.class);
                startActivity(intent);

                // TODO : use item data.
            }
        });

        return rootView;

    }
}

//data class
class ListViewItem {
    private String name;
    private String summary;
    private Drawable thumb_url;

    public void setIcon(Drawable icon) {
        thumb_url = icon ;
    }
    public void setTitle(String title) {
        name = title ;
    }
    public void setDesc(String desc) {
        summary = desc ;
    }

    public String getName() {
        return this.name;
    }

    public String getSummary() {
        return this.summary;
    }

    public Drawable getThumb_url() {
        return this.thumb_url;
    }
}

